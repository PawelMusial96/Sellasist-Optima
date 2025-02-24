using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.ModelsSellAsist.Documents;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.ModelsAplikacji;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Loader;
using System.Text;

namespace Sellasist_Optima.Pages.Zamówienia
{
    public class SalesOrdersModel : PageModel
    {
    private readonly IHttpClientFactory _clientFactory;
    private readonly ConfigurationContext _context;  // EF – do odczytu konfiguracji z Configuration.cshtml

    public SalesOrdersModel(IHttpClientFactory clientFactory, ConfigurationContext context)
    {
        _clientFactory = clientFactory;
        _context = context;
    }

    public List<Status> AllStatuses { get; set; } = new List<Status>();

    public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();

    [BindProperty]
    public string PayerCodeName { get; set; }

    [BindProperty]
    public int SelectedStatusId { get; set; }

    [BindProperty]
    public int SelectedNewStatusId { get; set; }

    public string SelectedStatusName { get; set; }


    public List<Order> Orders { get; set; } = new List<Order>();
    public List<AddressBill> AddressList { get; set; } = new List<AddressBill>();
    public List<Country> CountryList { get; set; } = new List<Country>();

    public string ResultMessage { get; set; }

    public async Task OnGetAsync()
    {
        var konfiguracja = await _context.KonfiguracjaAplikacji.FirstOrDefaultAsync();
        if (konfiguracja != null)
            {
                PayerCodeName = konfiguracja.PayerCodeName;
            }

        await LoadStatusesAsync();
        BuildStatusSelectList();
    }

    public Task<IActionResult> OnPostFetchOrders()
    {
        return OnPostFetchOrdersById(SelectedStatusId);
    }

    public async Task<IActionResult> OnPostFetchOrdersById(int selectedStatusId)
    {
        if (!AllStatuses.Any())
        {
            await LoadStatusesAsync();
        }
        BuildStatusSelectList();

        var chosenStatus = AllStatuses.FirstOrDefault(s => s.Id == selectedStatusId);
        SelectedStatusName = chosenStatus?.Name ?? "Unknown";

        await LoadOrdersByStatusAsync(SelectedStatusId);

        return Page();
    }

        public async Task<IActionResult> OnPostDownloadAndCreateDocs()
        {
            var konfiguracja = await _context.KonfiguracjaAplikacji.FirstOrDefaultAsync();
            if (konfiguracja == null)
            {
                konfiguracja = new KonfiguracjaAplikacji();
                _context.KonfiguracjaAplikacji.Add(konfiguracja);
            }

            if (!string.IsNullOrWhiteSpace(PayerCodeName))
            {
                konfiguracja.PayerCodeName = PayerCodeName;
            }

            await _context.SaveChangesAsync();

            await LoadStatusesAsync();
            BuildStatusSelectList();

            await LoadOrdersByStatusAsync(SelectedStatusId);

            if (Orders == null || !Orders.Any())
            {
                ResultMessage = "Brak zamówień do przetworzenia w tym statusie.";
                return Page();
            }

            var webApiConfig = await _context.WebApiClient.FirstOrDefaultAsync();
            if (webApiConfig == null)
            {
                ResultMessage = "Brak konfiguracji WebApiClient.";
                return Page();
            }

            int updatedCount = 0;

            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    // Logowanie do Optima
                    client.BaseAddress = new Uri(webApiConfig.Localhost);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", webApiConfig.TokenAPI);

                    var loginUrl = $"/api/LoginOptima?companyName={webApiConfig.CompanyName}";
                    var loginResp = await client.PostAsync(loginUrl, null);
                    if (!loginResp.IsSuccessStatusCode)
                    {
                        ResultMessage = $"Błąd logowania do Optima. Kod: {loginResp.StatusCode}";
                        return Page();
                    }

                    foreach (var order in Orders)
                    {
                        bool success = await CreateDocForOrder(client, order);    
                        if (success)
                        {
                            bool updateOk = await UpdateSellasistOrderStatus(order.Id, SelectedNewStatusId);
                            if (updateOk)
                            {
                                updatedCount++;
                            }
                        }
                    }
                    await LogoutOptima(client);
                }

                ResultMessage = $"Utworzono dokumenty 308 i zaktualizowano status dla {updatedCount} zamówień." +
                                $"(Stary status={SelectedStatusId}, nowy={SelectedNewStatusId}).";
            }
            catch (Exception ex)
            {
                ResultMessage = $"Wyjątek podczas przetwarzania zamówień: {ex.Message}";
            }
            Orders = null;
            return Page();
        }

        public async Task<IActionResult> OnPostCreateDoc(int orderId)
    {
        await LoadOrdersByStatusAsync(SelectedStatusId);

        var order = Orders.FirstOrDefault(o => o.Id == orderId.ToString());
        if (order == null)
        {
            ResultMessage = $"Nie odnaleziono zamówienia o ID={orderId}.";
            return await OnPostFetchOrdersById(SelectedStatusId);
        }

        var webApiConfig = await _context.WebApiClient.FirstOrDefaultAsync();
        if (webApiConfig == null)
        {
            ResultMessage = "Brak konfiguracji WebApiClient. Uzupełnij w sekcji Konfiguracja.";
            return await OnPostFetchOrdersById(SelectedStatusId);
        }

        string baseUrl = webApiConfig.Localhost;
        if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
        {
            baseUrl = "http://" + baseUrl;
        }
        string token = webApiConfig.TokenAPI;
        string companyName = webApiConfig.CompanyName;

        try
        {
            using (var client = _clientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var loginUrl = $"/api/LoginOptima?companyName={companyName}";
                var loginResp = await client.PostAsync(loginUrl, null);
                if (!loginResp.IsSuccessStatusCode)
                {
                    ResultMessage = $"Błąd logowania do Optima. Kod: {loginResp.StatusCode}";
                    return await OnPostFetchOrdersById(SelectedStatusId);
                    }
                    var success = await CreateDocForOrder(client, order);
                    if (!success)
                {
                    ResultMessage = $"Błąd podczas tworzenia dokumentu 308 (ID={orderId}).";
                    await LogoutOptima(client);
                    return await OnPostFetchOrdersById(SelectedStatusId);
                }

                ResultMessage = $"Dokument 308 poprawnie utworzony dla zamówienia ID={orderId}!";

                await LogoutOptima(client);
            }
        }
        catch (Exception ex)
        {
            ResultMessage = $"Wyjątek podczas tworzenia dokumentu 308: {ex.Message}";
        }

        return await OnPostFetchOrdersById(SelectedStatusId);
    }

    private async Task<bool> CreateDocForOrder(HttpClient client, Order order)
    {
        try
        {
                var konfiguracja = await _context.KonfiguracjaAplikacji.FirstOrDefaultAsync();
                var payerCodeOrName1 = string.IsNullOrEmpty(konfiguracja?.PayerCodeName) ? string.Empty : konfiguracja.PayerCodeName;

                var docUrl = "/api/Documents";
                //Adres Nabywacy
                var address = order.BillAddress;
                var countryInfo = address?.Country;
 
                bool isCompany = !string.IsNullOrWhiteSpace(address?.CompanyName);

                string payerName = isCompany ? address.CompanyName.Trim() : $"{address?.Name} {address?.Surname}".Trim();

                string payerVatNumber = address?.CompanyNip ?? "";

                string countryName = countryInfo?.Name ?? "Polska"; // lub countryInfo?.Code
                string? city = address?.City;
                string? street = address?.Street;
                string? postCode = address?.Postcode;
                string? houseNumber = address?.HomeNumber;
                string? flatNumber = address?.FlatNumber;

                ////Adres Odbiorcy
                //var addresShipment = order.ShipmentAddress;
                //var countryInfoShipment = addresShipment?.Country;

                //bool isCompanyShipment = !string.IsNullOrWhiteSpace(addresShipment?.CompanyName);

                //string payerNameShipment = isCompany ? addresShipment.CompanyName.Trim() : $"{address?.Name} {address?.Surname}".Trim();

                //string payerVatNumberShipment = addresShipment?.CompanyNip ?? "";

                //string countryNameShipment = countryInfo?.Name ?? "Polska"; // lub countryInfo?.Code
                //string? cityShipment = addresShipment?.City;
                //string? streetShipment = addresShipment?.Street;
                //string? postCodeShipment = addresShipment?.Postcode;
                //string? houseNumberShipment = addresShipment?.HomeNumber;
                //string? flatNumberShipment = addresShipment?.FlatNumber;


                var docData = new
            {
                type = 308,                           // Zamówienie sprzedaży
                foreignNumber = $"ZAM_{order.Id}",    //Wartośc numeru obcego
                calculatedOn = 1,                     // 1=netto
                paymentMethod = "przelew",            // przykładowo
                currency = order.Payment?.Currency ?? "PLN",       // waluta
                description = $"Zam. z Sellasist (ID={order.Id})", // opis
                status = 1,                           // do edycji
                sourceWarehouseId = 1,
                documentIssueDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),

                payer = new
                {
                    code = payerCodeOrName1,
                    name1 = payerCodeOrName1,
                    //type = 1,    // 1 – standardowy kontrahent
                    vatNumber = payerVatNumber,
                    country = countryName,
                    city,
                    street,
                    postCode,
                    houseNumber,
                    flatNumber,
                    email = order.Email
                },

                recipient = new
                {
                    code = payerCodeOrName1,
                    name1 = payerCodeOrName1,
                    //type = 1,    // 1 – standardowy kontrahent
                    vatNumber = payerVatNumber,
                    country = countryName,
                    city,
                    street,
                    postCode,
                    houseNumber,
                    flatNumber,
                    email = order.Email
                },

                elements = order.Carts?.Select(cartItem => new
                {
                    code = cartItem.Symbol ?? "ITEM",
                    quantity = cartItem.Quantity,
                    unitNetPrice = cartItem.Price,
                    TotalNetValue = cartItem.Price * cartItem.Quantity,
                    vatRate = 23.0,        // stała stawka VAT
                    setCustomValue = true
                })
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(docData),
                Encoding.UTF8,
                "application/json");

            var createResp = await client.PostAsync(docUrl, jsonContent);
            if (!createResp.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task LogoutOptima(HttpClient client)
    {
        var logoutUrl = "/api/LogoutOptima";
        await client.PostAsync(logoutUrl, null);
    }

    private async Task LoadStatusesAsync()
    {
        try
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo != null)
            {
                HttpClient client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage response = await client.GetAsync("/api/v1/statuses");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var statuses = JsonConvert.DeserializeObject<List<Status>>(jsonString);

                    if (statuses != null)
                    {
                        AllStatuses = statuses;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania statusów: {ex.Message}");
        }
    }

    private void BuildStatusSelectList()
    {
        StatusList = AllStatuses
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
            .ToList();
    }

    private async Task LoadOrdersByStatusAsync(int statusId)
    {
        Orders = new List<Order>();

        try
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo != null)
            {
                HttpClient client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                var dateFrom = "2020-11-20 09:08:13";
                var dateTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var endpoint =
                    $"/api/v1/orders_with_carts?status_id={statusId}" +
                    $"&date_from={Uri.EscapeDataString(dateFrom)}" +
                    $"&date_to={Uri.EscapeDataString(dateTo)}";

                var response = await client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<Order>>(jsonString);
                    if (orders != null)
                    {
                        Orders = orders;
                    }
                }
                else
                {
                    Console.WriteLine($"Błąd odpowiedzi API: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy pobieraniu zamówień: {ex.Message}");
        }
       }

    private async Task<bool> UpdateSellasistOrderStatus(string orderId, int newStatusId)
        {
            try
            {
                var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
                if (apiInfo == null)
                {
                    Console.WriteLine("Brak konfiguracji SellAsistAPI");
                    return false;
                }

                using (HttpClient client = _clientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri(apiInfo.ShopName);
                    client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                    var updateUrl = $"/api/v1/orders/{orderId}";

                    var body = new
                    {
                        id = orderId,
                        status = newStatusId
                    };

                    var jsonString = JsonConvert.SerializeObject(body);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(updateUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Błąd przy zmianie statusu. Kod: {response.StatusCode}");
                    }

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyjątek przy zmianie statusu: {ex.Message}");
                return false;
            }
        }

    }
}

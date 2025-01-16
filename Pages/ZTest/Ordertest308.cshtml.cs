using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.ModelsSellAsist.Documents;
using Sellasist_Optima.SellAsistModels;
using System.Net.Http.Headers;
using System.Text;

namespace Sellasist_Optima.Pages.ZTest
{
    public class Ordertest308Model : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;  // EF – do odczytu konfiguracji z Configuration.cshtml

        public Ordertest308Model(IHttpClientFactory clientFactory, ConfigurationContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        // Lista statusów z Sellasist
        public List<Status> AllStatuses { get; set; } = new List<Status>();

        // Do selecta (dropdown)
        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();

        // ID wybranego statusu (powi¹zany z dropdown)
        [BindProperty]
        public int SelectedStatusId { get; set; }

        // Do wyœwietlenia nazwy wybranego statusu
        public string SelectedStatusName { get; set; }

        // Lista zamówieñ pobranych z Sellasist
        public List<Order> Orders { get; set; } = new List<Order>();

        // Komunikat zwrotny dla u¿ytkownika
        public string ResultMessage { get; set; }

        // -------------------------------------
        // OnGet, pobieramy listê statusów
        // -------------------------------------
        public async Task OnGetAsync()
        {
            await LoadStatusesAsync();
            BuildStatusSelectList();
        }

        // -------------------------------------
        // Odczyt zamówieñ wg statusu (opcjonalny)
        // -------------------------------------
        public Task<IActionResult> OnPostFetchOrders()
        {
            return OnPostFetchOrdersById(SelectedStatusId);
        }

        public async Task<IActionResult> OnPostFetchOrdersById(int selectedStatusId)
        {
            // Upewnij siê, ¿e mamy statusy
            if (!AllStatuses.Any())
            {
                await LoadStatusesAsync();
            }
            BuildStatusSelectList();

            // ZnajdŸ nazwê wybranego statusu
            var chosenStatus = AllStatuses.FirstOrDefault(s => s.Id == selectedStatusId);
            SelectedStatusName = chosenStatus?.Name ?? "Unknown";

            // Pobierz zamówienia w wybranym statusie
            await LoadOrdersByStatusAsync(SelectedStatusId);

            return Page();
        }

        // -------------------------------------
        // Handler: pobierz i UTWÓRZ dokumenty 308
        // -------------------------------------
        /// <summary>
        /// 1) Pobiera wszystkie zamówienia z wybranego statusu
        /// 2) Dla ka¿dego zamówienia tworzy dokument 308
        /// 3) Nie wyœwietla danych o zamówieniach
        /// </summary>
        public async Task<IActionResult> OnPostDownloadAndCreateDocs()
        {
            // Za³aduj statusy i listê do dropdown
            await LoadStatusesAsync();
            BuildStatusSelectList();

            // Pobierz zamówienia w danym statusie
            await LoadOrdersByStatusAsync(SelectedStatusId);

            // Jeœli brak zamówieñ -> koniec
            if (Orders == null || !Orders.Any())
            {
                ResultMessage = "Brak zamówieñ do przetworzenia w tym statusie.";
                return Page();
            }

            // === Wczytanie konfiguracji z bazy (z Configuration.cshtml) ===
            var webApiConfig = await _context.WebApiClient.FirstOrDefaultAsync();
            if (webApiConfig == null)
            {
                ResultMessage = "Brak konfiguracji WebApiClient. Uzupe³nij w sekcji Konfiguracja.";
                return Page();
            }

            // Tutaj pobieramy parametry z WebApiClient:
            //   Localhost, CompanyName, TokenAPI, DatabaseName, itp.
            // Dla przyk³adu przyjmujemy, ¿e "Localhost" to nasz BaseAddress,
            // "CompanyName" to np. FIRMA_DEMO (zmienna),
            // "TokenAPI" to Bearer token.
            // Oczywiœcie mo¿esz zmieniæ w zale¿noœci od konwencji w swojej bazie.
            string baseUrl = webApiConfig.Localhost;      // e.g. "http://localhost:6462"
            string token = webApiConfig.TokenAPI;         // e.g. "some-long-bearer-token"
            string companyName = webApiConfig.CompanyName; // e.g. "FIRMA_DEMO"

            // (Opcjonalnie dopisz http:// jeœli user wpisa³ samo "localhost:6462")
            if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
            {
                baseUrl = "http://" + baseUrl;
            }

            int createdCount = 0;

            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    // Ustaw BaseAddress + Bearer
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    // Logowanie do Comarch
                    // Zamiast "...?companyName=FIRMA_DEMO", u¿ywamy wczytanej wartoœci "companyName"
                    var loginUrl = $"/api/LoginOptima?companyName={companyName}";
                    var loginResp = await client.PostAsync(loginUrl, null);
                    if (!loginResp.IsSuccessStatusCode)
                    {
                        ResultMessage = $"B³¹d logowania do Optima. Kod: {loginResp.StatusCode}";
                        return Page();
                    }

                    // Dla ka¿dego zamówienia twórz 308
                    foreach (var order in Orders)
                    {
                        bool success = await CreateDocForOrder(client, order);
                        if (success) createdCount++;
                    }

                    // Wylogowanie
                    await LogoutOptima(client);
                }

                ResultMessage = $"Utworzono dokument 308 dla {createdCount} zamówieñ (status={SelectedStatusId}).";
            }
            catch (Exception ex)
            {
                ResultMessage = $"Wyj¹tek podczas przetwarzania zamówieñ: {ex.Message}";
            }

            // Strona nie wyœwietla listy
            Orders = null;
            return Page();
        }

        // -------------------------------------
        // Handler: UTWÓRZ dokument 308 dla jednego zamówienia
        // -------------------------------------
        public async Task<IActionResult> OnPostCreateDoc(int orderId)
        {
            // Upewnij siê, ¿e mamy zamówienia
            await LoadOrdersByStatusAsync(SelectedStatusId);

            var order = Orders.FirstOrDefault(o => o.Id == orderId.ToString());
            if (order == null)
            {
                ResultMessage = $"Nie odnaleziono zamówienia o ID={orderId}.";
                return await OnPostFetchOrdersById(SelectedStatusId);
            }

            // Znów wczytaj konfiguracjê z bazy
            var webApiConfig = await _context.WebApiClient.FirstOrDefaultAsync();
            if (webApiConfig == null)
            {
                ResultMessage = "Brak konfiguracji WebApiClient. Uzupe³nij w sekcji Konfiguracja.";
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
                    // Autoryzacja
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    // Logowanie
                    var loginUrl = $"/api/LoginOptima?companyName={companyName}";
                    var loginResp = await client.PostAsync(loginUrl, null);
                    if (!loginResp.IsSuccessStatusCode)
                    {
                        ResultMessage = $"B³¹d logowania do Optima. Kod: {loginResp.StatusCode}";
                        return await OnPostFetchOrdersById(SelectedStatusId);
                    }

                    // Tworzymy dokument 308
                    var success = await CreateDocForOrder(client, order);
                    if (!success)
                    {
                        ResultMessage = $"B³¹d podczas tworzenia dokumentu 308 (ID={orderId}).";
                        await LogoutOptima(client);
                        return await OnPostFetchOrdersById(SelectedStatusId);
                    }

                    // Jeœli OK
                    ResultMessage = $"Dokument 308 poprawnie utworzony dla zamówienia ID={orderId}!";

                    // Wylogowanie
                    await LogoutOptima(client);
                }
            }
            catch (Exception ex)
            {
                ResultMessage = $"Wyj¹tek podczas tworzenia dokumentu 308: {ex.Message}";
            }

            return await OnPostFetchOrdersById(SelectedStatusId);
        }

        // -------------------------------------
        // Metoda tworz¹ca dokument 308 (private)
        // -------------------------------------
        private async Task<bool> CreateDocForOrder(HttpClient client, Order order)
        {
            try
            {
                // Relative path, since we set BaseAddress
                var docUrl = "/api/Documents";

                // Przyk³adowy mapping JSON
                var docData = new
                {
                    type = 308,                           // Zamówienie sprzeda¿y
                    foreignNumber = $"ZAM_{order.Id}",
                    calculatedOn = 1,                     // 1=netto
                    paymentMethod = "przelew",            // przyk³adowo
                    currency = order.Payment?.Currency ?? "PLN",
                    description = $"Zam. z Sellasist (ID={order.Id})",
                    status = 1,                           // do edycji
                    sourceWarehouseId = 1,
                    documentIssueDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),

                    payer = new
                    {
                        code = "!NIEOKREŒLONY!",
                        name1 = "!NIEOKREŒLONY!"
                    },

                    elements = order.Carts?.Select(cartItem => new
                    {
                        code = cartItem.Symbol ?? "ITEM",
                        quantity = cartItem.Quantity,
                        unitNetPrice = cartItem.Price,
                        TotalNetValue = cartItem.Price * cartItem.Quantity,
                        vatRate = 23.0,        // sta³a stawka VAT
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
                    // Tu mo¿na zalogowaæ b³¹d w razie potrzeby
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // -------------------------------------
        // Wylogowanie – relative path
        // -------------------------------------
        private async Task LogoutOptima(HttpClient client)
        {
            var logoutUrl = "/api/LogoutOptima";
            await client.PostAsync(logoutUrl, null);
        }

        // -------------------------------------
        // Metody pomocnicze do pobierania z Sellasist
        // -------------------------------------
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
                Console.WriteLine($"B³¹d pobierania statusów: {ex.Message}");
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
                        Console.WriteLine($"B³¹d odpowiedzi API: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B³¹d przy pobieraniu zamówieñ: {ex.Message}");
            }
        }
    }
}

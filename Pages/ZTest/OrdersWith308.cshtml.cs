using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// Twoje przestrzenie nazw do modeli Sellasist i bazy danych:
using Sellasist_Optima.ModelsSellAsist.Documents;  // np. klasa Order
using Sellasist_Optima.SellAsistModels;            // np. klasa Status
using Sellasist_Optima.BazyDanych;                 // np. ConfigurationContext

namespace Sellasist_Optima.Pages.ZTest
{
    public class OrdersWith308Model : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;  // EF – do odczytu konfiguracji Sellasist

        public OrdersWith308Model(IHttpClientFactory clientFactory, ConfigurationContext context)
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

        // Przy wejœciu na stronê pobieramy listê statusów do dropdown
        public async Task OnGetAsync()
        {
            await LoadStatusesAsync();
            BuildStatusSelectList();
        }

        // Obs³uga pobierania zamówieñ po statusie
        public Task<IActionResult> OnPostFetchOrders()
        {
            return OnPostFetchOrdersById(SelectedStatusId);
        }

        public async Task<IActionResult> OnPostFetchOrdersById(int selectedStatusId)
        {
            // Upewnij siê, ¿e mamy za³adowane statusy
            if (!AllStatuses.Any())
            {
                await LoadStatusesAsync();
            }
            BuildStatusSelectList();

            // ZnajdŸ nazwê wybranego statusu
            var chosenStatus = AllStatuses.FirstOrDefault(s => s.Id == selectedStatusId);
            SelectedStatusName = chosenStatus?.Name ?? "Unknown";

            // Teraz pobierz zamówienia dla wybranego statusu
            await LoadOrdersByStatusAsync(SelectedStatusId);

            return Page();
        }

        /// <summary>
        /// G³ówny handler tworzenia dokumentu 308 (Zamówienie sprzeda¿y) w Comarch Optima.
        /// Wywo³ywany przyciskiem „Utwórz 308” dla konkretnego zamówienia.
        /// </summary>
        //public async Task<IActionResult> OnPostCreateDoc(int orderId)
        //{
        //    await LoadOrdersByStatusAsync(SelectedStatusId);
        //    // 1) Opcjonalnie: znajdŸ dane zamówienia z bie¿¹cej listy albo pobierz je ponownie
        //    //var order = Orders.FirstOrDefault(o => o.Id == orderId);
        //    var order = Orders.FirstOrDefault(o => o.Id == orderId.ToString());

        //    // Jeœli nie ma zamówienia w lokalnej liœcie, mo¿na je pobraæ ponownie lub zwróciæ b³¹d
        //    if (order == null)
        //    {
        //        ResultMessage = $"Nie odnaleziono zamówienia o ID={orderId} w aktualnej liœcie!";
        //        return await OnPostFetchOrdersById(SelectedStatusId);
        //    }

        //    // ---------------------------------------------------------
        //    // 2) Wywo³anie API Comarch Optima w celu utworzenia dok. 308
        //    // ---------------------------------------------------------
        //    // Przyk³adowo mo¿na trzymaæ dane w appsettings lub bazie:
        //    const string COMARCH_API_BASE = "http://localhost:6462";
        //    const string BEARER_TOKEN =
        //        "KynUgS2Zx6j5mH5HO-r3MncWP1S3cvwbFVWLqPHnbPfos2f7qYcZKi2lcmhO53jvwO_Wa5uJcNdDcC4zrabWlIroP3ROJfT3E-RlearXn-TBNjuDXYQXdV9NiInLtz8W9x0AWvzNSP_iMrQlbCxgYw-JV0ny0ghMYozg4ZcnCik9Dc7uwRSrzIIyKMsRnotz5sV6I5fYwJlxOGLLRNpS41-FXGkLiBDWX4utWc8_w7tlpS8JrDcNamLUQOl07PN1DQVTqzFP1h9dna1UL_LQYofOBl9AA4ou5qYw_a7QvvE-rP0-_AJBp21jd-UEs_D3";
        //        // (skrócone dla czytelnoœci)

        //    try
        //    {
        //        using (var client = _clientFactory.CreateClient())
        //        {
        //            // Ustawienie nag³ówka typu Bearer
        //            client.DefaultRequestHeaders.Authorization =
        //                new AuthenticationHeaderValue("Bearer", BEARER_TOKEN);

        //            // (1) Logowanie do Optima
        //            var loginUrl = $"{COMARCH_API_BASE}/api/LoginOptima?companyName=FIRMA_DEMO";
        //            var loginResp = await client.PostAsync(loginUrl, null);
        //            if (!loginResp.IsSuccessStatusCode)
        //            {
        //                ResultMessage = $"B³¹d logowania do Optima. Kod: {loginResp.StatusCode}";
        //                return await OnPostFetchOrdersById(SelectedStatusId);
        //            }

        //            // (2) Utworzenie dokumentu typu 308 (Zamówienie sprzeda¿y)
        //            var docUrl = $"{COMARCH_API_BASE}/api/Documents";

        //            // Mapujemy zamówienie z Sellasist na strukturê dokumentu:
        //            // Dla uproszczenia tworzymy jedn¹ pozycjê. Mo¿na rozbudowaæ o wszystkie pozycje z zamówienia.
        //            var docData = new
        //            {
        //                type = 308,  // Zamówienie sprzeda¿y
        //                foreignNumber = $"ZAM_{order.Id}_FromApp",
        //                calculatedOn = 1,   // 1 = netto
        //                paymentMethod = "przelew",
        //                currency = "PLN",
        //                description = $"Zamówienie utworzone z Sellasist, ID={order.Id}",
        //                status = 1,         // 1 = do edycji
        //                sourceWarehouseId = 1,
        //                documentIssueDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),

        //                // Minimalne dane kontrahenta
        //                payer = new
        //                {
        //                    code = "!NIEOKREŒLONY!",
        //                    name1 = "!NIEOKREŒLONY!"
        //                },

        //                // Przyk³ad: bierzemy wszystkie pozycje z zamówienia
        //                elements = order.Carts?.Select(cartItem => new {
        //                    code = cartItem.Name ?? "ITEM",
        //                    quantity = cartItem.Quantity,
        //                    vatRate = 23.0,
        //                    setCustomValue = false
        //                })
        //            };

        //            var jsonContent = new StringContent(
        //                System.Text.Json.JsonSerializer.Serialize(docData),
        //                Encoding.UTF8,
        //                "application/json"
        //            );

        //            var createResp = await client.PostAsync(docUrl, jsonContent);
        //            if (!createResp.IsSuccessStatusCode)
        //            {
        //                ResultMessage = $"B³¹d podczas tworzenia dokumentu 308. Kod: {createResp.StatusCode}";

        //                // (3) Opcjonalne wylogowanie
        //                await LogoutOptima(client, COMARCH_API_BASE);
        //                return await OnPostFetchOrdersById(SelectedStatusId);
        //            }

        //            // Jeœli OK:
        //            var responseString = await createResp.Content.ReadAsStringAsync();
        //            ResultMessage = $"Dokument 308 poprawnie utworzony dla zamówienia ID={orderId}! " +
        //                            $"OdpowiedŸ: {responseString}";

        //            // (3) Wylogowanie z Optima
        //            await LogoutOptima(client, COMARCH_API_BASE);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ResultMessage = $"Wyj¹tek podczas tworzenia dokumentu 308: {ex.Message}";
        //    }

        //    // Na koniec odœwie¿amy stronê z komunikatem
        //    return await OnPostFetchOrdersById(SelectedStatusId);
        //}

        // Metoda pomocnicza: wylogowanie z Optima

        public async Task<IActionResult> OnPostCreateDoc(int orderId)
        {
            await LoadOrdersByStatusAsync(SelectedStatusId);

            // Szukamy zamówienia o konkretnym ID (UWAGA: w SellAsist order.Id jest typu string)
            var order = Orders.FirstOrDefault(o => o.Id == orderId.ToString());

            if (order == null)
            {
                ResultMessage = $"Nie odnaleziono zamówienia o ID={orderId} w aktualnej liœcie!";
                return await OnPostFetchOrdersById(SelectedStatusId);
            }

            const string COMARCH_API_BASE = "http://localhost:6462";
            const string BEARER_TOKEN =
"jwROrpIaort4R-A4lwAoPHdZ_WbxfJA6bF6WPJQubXW6Oiy3Tso8pzvOvF-nuJgfu20jcBrLEXTpmdly8vVRAYkpQY8qzCY81HEyeg4Wy6NDCTx7UcYRi77Ki-35iR-VjFAAonY0m4UNmgXw44S1tNzybtNoFav7i4qLNTKqGzOJz3lE2ks-ik5a-X2h3oXcTj4uQhhpSzjhpINk9VZ1chbMwGGvALKShRZY-lomOCLbsZkOZsejVnSUkdCpQK9cow76u8ikjAFnVRQzrEoEraknmUFuBxfTgUcbNMB27d7tM7HJXTYYEZ00439KxNVx";
            // (przyk³adowy token skrócony dla czytelnoœci)
            ;

            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    // 1) Autoryzacja w Comarch Optima
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", BEARER_TOKEN);

                    var loginUrl = $"{COMARCH_API_BASE}/api/LoginOptima?companyName=FIRMA_DEMO";
                    var loginResp = await client.PostAsync(loginUrl, null);
                    if (!loginResp.IsSuccessStatusCode)
                    {
                        ResultMessage = $"B³¹d logowania do Optima. Kod: {loginResp.StatusCode}";
                        return await OnPostFetchOrdersById(SelectedStatusId);
                    }

                    // 2) Tworzenie dokumentu 308 (Zamówienie sprzeda¿y)
                    var docUrl = $"{COMARCH_API_BASE}/api/Documents";

                    // MAPPING: wartoœci z SellAsist -> dokument w Optimie
                    var docData = new
                    {
                        type = 308,                         // (hard-coded) Zamówienie sprzeda¿y
                        foreignNumber = $"ZAM_{order.Id}",  // Przyk³adowy numer obcy
                        calculatedOn = 1,                   // (hard-coded) 1 = netto 0 = brutto
                        //paymentMethod = order.Payment?.Name ?? "przelew",    // Z SellAsist -> Payment.Name
                        paymentMethod = "przelew",
                        currency = order.Payment?.Currency ?? "PLN",         // Z SellAsist -> Payment.Currency
                        description = $"Zam. z Sellasist (ID={order.Id})",   // Opis
                        status = 1,                        // (hard-coded) 1 = do edycji
                        sourceWarehouseId = 1,             // (hard-coded) przyk³adowy magazyn
                        documentIssueDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),

                        // Dane kontrahenta (przyk³adowo „!NIEOKREŒLONY!”)
                        payer = new
                        {
                            code = "!NIEOKREŒLONY!",
                            name1 = "!NIEOKREŒLONY!"
                        },

                        
                        // Pozycje dokumentu – tutaj mapujemy bezpoœrednio z listy Carts
                        elements = order.Carts?.Select(cartItem => new
                        {
                            code = cartItem.Symbol ?? "ITEM",    // Z SellAsist -> CartItem.Symbol
                            quantity = cartItem.Quantity,        // Z SellAsist -> CartItem.Quantity
                            unitNetPrice = cartItem.Price,
                            TotalNetValue = cartItem.Quantity * cartItem.Price,
                            /*unitGrossPrice = cartItem.Price,*/// Z SellAsist -> CartItem.Price (netto)
                            vatRate = 23.0,                     // (hard-coded) Stawka VAT
                            setCustomValue = true
                        })
                    };

                    var jsonContent = new StringContent(
                        System.Text.Json.JsonSerializer.Serialize(docData),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var createResp = await client.PostAsync(docUrl, jsonContent);
                    if (!createResp.IsSuccessStatusCode)
                    {
                        ResultMessage = $"B³¹d podczas tworzenia dokumentu 308. Kod: {createResp.StatusCode}";
                        // Wylogowanie (opcjonalne)
                        await LogoutOptima(client, COMARCH_API_BASE);
                        return await OnPostFetchOrdersById(SelectedStatusId);
                    }

                    // Jeœli OK:
                    var responseString = await createResp.Content.ReadAsStringAsync();
                    ResultMessage = $"Dokument 308 poprawnie utworzony dla zamówienia ID={orderId}! " +
                                    $"OdpowiedŸ: {responseString}";

                    // 3) Wylogowanie z Optima
                    await LogoutOptima(client, COMARCH_API_BASE);
                }
            }
            catch (Exception ex)
            {
                ResultMessage = $"Wyj¹tek podczas tworzenia dokumentu 308: {ex.Message}";
            }

            // Odœwie¿amy stronê
            return await OnPostFetchOrdersById(SelectedStatusId);
        }

        private async Task LogoutOptima(HttpClient client, string baseUrl)
        {
            var logoutUrl = $"{baseUrl}/api/LogoutOptima";
            await client.PostAsync(logoutUrl, null);
        }

        //private async Task LogoutOptima(HttpClient client, string baseUrl)
        //{
        //    var logoutUrl = $"{baseUrl}/api/LogoutOptima";
        //    await client.PostAsync(logoutUrl, null);
        //}

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

                    var endpoint = $"/api/v1/orders_with_carts?status_id={statusId}" +
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

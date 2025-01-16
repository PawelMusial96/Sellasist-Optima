using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;                  // <-- Potrzebne do .Any(), .Select(), FirstOrDefault() itd.
using System.Net.Http;
using System.Threading.Tasks;
using Sellasist_Optima.ModelsSellAsist.Documents;  // Definicja klasy Order
using Sellasist_Optima.SellAsistModels;            // Definicja klasy Status (z Sellasist)
using Sellasist_Optima.BazyDanych;                 // Definicja ConfigurationContext lub SellAsistAPI

namespace Sellasist_Optima.Pages.ZTest
{
    public class TestZamówieńSellAsistModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;  // Dodajemy pole kontekstu

        // Konstruktor – wstrzykujemy zarówno IHttpClientFactory, jak i ConfigurationContext
        public TestZamówieńSellAsistModel(IHttpClientFactory clientFactory, ConfigurationContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        // Lista statusów pobrana z Sellasist
        public List<SellAsistModels.Status> AllStatuses { get; set; }
            = new List<SellAsistModels.Status>();

        // Lista do wyświetlenia w dropdownie
        public List<SelectListItem> StatusList { get; set; }
            = new List<SelectListItem>();

        // Wybrany status (ID) – zbindowany z dropdowna
        [BindProperty]
        public int SelectedStatusId { get; set; }

        // Nazwa wybranego statusu (do wyświetlenia)
        public string SelectedStatusName { get; set; }

        // Lista zamówień pobrana z Sellasist, przefiltrowana po statusie
        public List<Order> Orders { get; set; } = new List<Order>();

        // GET: wywoływany przy wejściu na stronę
        public async Task OnGetAsync()
        {
            await LoadStatusesAsync();
            BuildStatusSelectList();
        }

        public Task<IActionResult> OnPostFetchOrders()
        {
            return OnPostFetchOrdersById(SelectedStatusId);
        }

        // POST: wywoływany przy submit z dropdowna (handler=FetchOrders)
        public async Task<IActionResult> OnPostFetchOrdersById(int selectedStatusId)
        {
            // Jeśli statusy nie zostały jeszcze pobrane (lub są puste), to pobierz je
            if (!AllStatuses.Any())
            {
                await LoadStatusesAsync();
            }

            // Zbuduj listę select
            BuildStatusSelectList();

            // Znajdź nazwę wybranego statusu
            var chosenStatus = AllStatuses.FirstOrDefault(s => s.Id == selectedStatusId);
            SelectedStatusName = chosenStatus?.Name ?? "Unknown";

            // Pobierz zamówienia z API
            await LoadOrdersByStatusAsync(SelectedStatusId);

            return Page();
        }

        private async Task LoadStatusesAsync()
        {
            try
            {
                // Pobierz dane konfiguracyjne API z bazy (np. pierwszy rekord)
                var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
                if (apiInfo != null)
                {
                    // Utwórz klienta HTTP
                    HttpClient client = _clientFactory.CreateClient();

                    // Ustaw adres bazowy (np. "https://wszystkoinic.sellasist.pl")
                    client.BaseAddress = new Uri(apiInfo.ShopName);

                    // Dodaj nagłówek z kluczem API
                    client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                    // Wywołaj endpoint "/api/v1/statuses"
                    HttpResponseMessage response = await client.GetAsync("/api/v1/statuses");
                    if (response.IsSuccessStatusCode)
                    {
                        // Odczytaj treść odpowiedzi
                        var jsonString = await response.Content.ReadAsStringAsync();

                        // Zdeserializuj do listy Status
                        var statuses = JsonConvert.DeserializeObject<List<SellAsistModels.Status>>(jsonString);

                        if (statuses != null)
                        {
                            AllStatuses = statuses;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Obsługa wyjątków
                Console.WriteLine($"Błąd przy pobieraniu statusów: {ex.Message}");
            }
        }

        private void BuildStatusSelectList()
        {
            // Konwertuj AllStatuses do listy SelectListItem do dropdowna
            StatusList = AllStatuses
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
        }

        private async Task LoadOrdersByStatusAsync(int statusId)
        {
            try
            {
                var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
                if (apiInfo != null)
                {
                    // Utwórz klienta
                    HttpClient client = _clientFactory.CreateClient();
                    client.BaseAddress = new Uri(apiInfo.ShopName);
                    client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                    // Ustal "date_from" (stała zgodnie z sugestią) oraz "date_to" (aktualna data).
                    var dateFrom = "2020-11-20 09:08:13";
                    var dateTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Złóż endpoint z parametrami
                    var endpoint = $"/api/v1/orders_with_carts?status_id={statusId}" +
                                   $"&date_from={Uri.EscapeDataString(dateFrom)}" +
                                   $"&date_to={Uri.EscapeDataString(dateTo)}";

                    // Wywołaj GET
                    var response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        // Deserializacja z użyciem Newtonsoft.Json
                        var orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(jsonString);

                        if (orders != null)
                        {
                            Orders = orders;
                        }
                    }
                    else
                    {
                        // Tu możesz dodać logikę w razie błędu np. 404, 401, 500 itd.
                        Console.WriteLine($"Błąd odpowiedzi API: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching orders: {ex.Message}");
                // Obsłuż wyjątki według potrzeb (logowanie, komunikat dla użytkownika itp.)
            }
        }

    }
}
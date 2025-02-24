using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sellasist_Optima.ModelsSellAsist.Documents;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.BazyDanych;

namespace Sellasist_Optima.Pages.ZTest
{
    public class TestZamówieńSellAsistModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;

        public TestZamówieńSellAsistModel(IHttpClientFactory clientFactory, ConfigurationContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        public List<SellAsistModels.Status> AllStatuses { get; set; } = new List<SellAsistModels.Status>();

        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public int SelectedStatusId { get; set; }

        public string SelectedStatusName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
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

            // Tutaj pobieramy wszystkie zamówienia dla wybranego statusu
            await LoadOrdersByStatusAsync(SelectedStatusId);

            return Page();
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
                Console.WriteLine($"Błąd przy pobieraniu statusów: {ex.Message}");
            }
        }

        private void BuildStatusSelectList()
        {
            StatusList = AllStatuses
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
        }

        /// <summary>
        /// Pobiera wszystkie zamówienia (z podziałem na strony) z SellAsist
        /// o zadanym statusie, i zapisuje do Orders.
        /// </summary>
        private async Task LoadOrdersByStatusAsync(int statusId)
        {
            try
            {
                var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
                if (apiInfo != null)
                {
                    HttpClient client = _clientFactory.CreateClient();
                    client.BaseAddress = new Uri(apiInfo.ShopName);
                    client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                    Orders = new List<Order>();

                    // Zakres dat - np. od 2020 do dziś
                    var dateFrom = "2020-11-20 09:08:13";
                    var dateTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Parametry stronicowania
                    int offset = 0;
                    const int limit = 200; // Możesz ustawić większy limit, jeśli SellAsist pozwala

                    while (true)
                    {
                        // Złożenie endpointu z parametrami status_id, offset, limit, date_from/do
                        var endpoint =
                            $"/api/v1/orders_with_carts?status_id={statusId}" +
                            $"&date_from={Uri.EscapeDataString(dateFrom)}" +
                            $"&date_to={Uri.EscapeDataString(dateTo)}" +
                            $"&offset={offset}" +
                            $"&limit={limit}";

                        var response = await client.GetAsync(endpoint);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Błąd odpowiedzi API: {response.StatusCode}");
                            break;
                        }

                        var jsonString = await response.Content.ReadAsStringAsync();
                        var partialOrders = JsonConvert.DeserializeObject<List<Order>>(jsonString);

                        if (partialOrders == null || partialOrders.Count == 0)
                        {
                            // Brak dalszych wyników
                            break;
                        }

                        // Dodaj do głównej listy
                        Orders.AddRange(partialOrders);

                        // Jeżeli mniej wyników niż limit, to znaczy, że to ostatnia strona
                        if (partialOrders.Count < limit)
                        {
                            break;
                        }

                        // Przejdź do kolejnej strony
                        offset += limit;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching orders: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.WebApiModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Sellasist_Optima.Pages.Zamówienia
{
    public class SalesOrdersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationContext _context;

        public SalesOrdersModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }


        public List<Status> Statuses { get; set; } = new List<Status>();

        [BindProperty]
        public int SelectedStatusId { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await FetchStatusesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostFetchOrdersAsync()
        {
            await FetchStatusesAsync();
            await FetchOrdersAsync(SelectedStatusId);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateSalesOrdersAsync()
        {
            await FetchStatusesAsync();
            await FetchOrdersAsync(SelectedStatusId);
            await CreateSalesOrdersAsync();
            return RedirectToPage();
        }

        private async Task FetchStatusesAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo == null)
            {
                Message = "SellAsist API settings not configured.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiInfo.ShopName);
            client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

            HttpResponseMessage response = await client.GetAsync("/api/v1/statuses");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Statuses = JsonConvert.DeserializeObject<List<Status>>(jsonResponse);
            }
            else
            {
                Message = $"Error fetching statuses: {response.ReasonPhrase}";
            }
        }

        private async Task FetchOrdersAsync(int statusId)
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync(); ;
            if (apiInfo == null)
            {
                Message = "SellAsist API settings not configured.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiInfo.ShopName);
            client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

            HttpResponseMessage response = await client.GetAsync($"/api/v1/orders_with_carts?status_id={statusId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Orders = JsonConvert.DeserializeObject<List<Order>>(content);
            }
            else
            {
                Message = $"Error fetching orders: {response.ReasonPhrase}";
            }
        }

        private async Task CreateSalesOrdersAsync()
        {
            var webApiClient = _context.WebApiClient.FirstOrDefault();
            if (webApiClient == null)
            {
                Message = "WebAPI settings not configured.";
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(webApiClient.Localhost);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", webApiClient.TokenAPI);

            foreach (var order in Orders)
            {
                var salesOrder = MapOrderToSalesOrder(order);

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(salesOrder), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("sales_orders", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    Message += $"Error creating sales order for Order ID {order.id}: {response.ReasonPhrase}\n";
                }
            }

            if (string.IsNullOrEmpty(Message))
            {
                Message = "Sales orders created successfully.";
            }
        }

        private SalesOrder MapOrderToSalesOrder(Order order)
        {
            return new SalesOrder
            {
                CustomerName = order.customer.name,
                CustomerEmail = order.customer.email,
                TotalPrice = order.total_price,
                OrderDate = order.date,
                Items = order.cart_items.Select(ci => new SalesOrderItem
                {
                    ProductId = ci.product_id,
                    ProductName = ci.product_name,
                    Quantity = ci.quantity,
                    UnitPrice = ci.price
                }).ToList() ?? new List<SalesOrderItem>()
            };
        }
    }
}
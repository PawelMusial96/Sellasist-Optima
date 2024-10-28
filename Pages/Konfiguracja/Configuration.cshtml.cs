using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.Models;
using Sellasist_Optima.WebApiModels;

namespace Sellasist_Optima.Pages.Konfiguracja
{
    public class ConfigurationModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;
        private readonly ConfigurationContext _context2;

        [BindProperty]
        public SellAsistAPI SellAsistAPI { get; set; }

        public WebApiClient WebApiClient { get; set; }
        public List<SellAsistAPI> AllSellAsistAPI { get; set; } = new List<SellAsistAPI>();

        public List<WebApiClient> AllWebApiClient { get; set; } = new List<WebApiClient>();

        [BindProperty]
        public string KeyApi { get; set; }
        [BindProperty]
        public string Login { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public ConfigurationModel(IHttpClientFactory clientFactory, ConfigurationContext context, ConfigurationContext context2)
        {
            _clientFactory = clientFactory;
            _context = context;
            _context2 = context2;
        }

        public async Task OnGetAsync()
        {
            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
        }

        public async Task<IActionResult> OnPostConnectAPIAsync()
        {
            // Sellasist API Logic
            var client = _clientFactory.CreateClient("SellAsistApiClient");
            client.BaseAddress = new Uri(SellAsistAPI.ShopName);
            client.DefaultRequestHeaders.Add("apiKey", SellAsistAPI.KeyAPI);

            HttpResponseMessage response = await client.GetAsync("endpoint");
            bool exists = await _context.SellAsistAPI.AnyAsync(api => api.ShopName == SellAsistAPI.ShopName && api.KeyAPI == SellAsistAPI.KeyAPI);

            if (!exists)
            {
                _context.SellAsistAPI.Add(SellAsistAPI);
                int saved = await _context.SaveChangesAsync();
                Message = saved > 0 ? "Sellasist API connection saved successfully" : "Sellasist API connection failed to save";
            }
            else
            {
                Message = "This Sellasist API already exists.";
            }

            // Web API Logic (login validation, etc.)
            if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
            {
                // Perform Web API connection logic here.
                // For demonstration, we'll assume it just appends to the message.
                Message += " | Web API connected.";
            }

            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostConnectWebAPIAsync()
        {
            AllWebApiClient = await _context.WebApiClient.ToListAsync();
            return Page();
        }


        }
}
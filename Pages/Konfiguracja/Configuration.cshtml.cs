using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.Models;
using Sellasist_Optima.WebApiModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Sellasist_Optima.Pages.Konfiguracja
{
    public class ConfigurationModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigurationContext _context;

        [BindProperty]
        [ValidateNever]
        public SellAsistAPI SellAsistAPI { get; set; }

        [BindProperty]
        [ValidateNever]
        public WebApiClient WebApiClient { get; set; }

        public List<SellAsistAPI> AllSellAsistAPI { get; set; } = new List<SellAsistAPI>();
        public List<WebApiClient> AllWebApiClient { get; set; } = new List<WebApiClient>();

        public string Message { get; set; }

        public ConfigurationModel(IHttpClientFactory clientFactory, ConfigurationContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
            AllWebApiClient = await _context.WebApiClient.ToListAsync();
        }

        public async Task<IActionResult> OnPostConnectWebAPIAsync()
        {
            try
            {
                bool exists = await _context.WebApiClient.AnyAsync(w =>
                    w.Username == WebApiClient.Username &&
                    w.Password == WebApiClient.Password &&
                    w.Grant_type == WebApiClient.Grant_type &&
                    w.TokenAPI == WebApiClient.TokenAPI);

                if (!exists)
                {
                    _context.WebApiClient.Add(WebApiClient);
                    int saved = await _context.SaveChangesAsync();
                    Message = saved > 0 ? "Web API connection saved successfully." : "Failed to save Web API connection.";
                }
                else
                {
                    Message = "This Web API client already exists.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error saving Web API client: {ex.Message}";
            }

            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
            AllWebApiClient = await _context.WebApiClient.ToListAsync();
            return Page();
        }

        //public async Task<IActionResult> OnGetEditAsync(int id)
        //{
        //    SellAsistAPI = await _context.SellAsistAPI.FindAsync(id);
        //    if (SellAsistAPI == null)
        //    {
        //        return NotFound();
        //    }

        //    //await LoadDataAsync();
        //    return Page();
        //}

        public async Task<IActionResult> OnPostConnectAPIAsync()
        {
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

            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();


            return Page();
        }
    }
}
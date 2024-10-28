using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sellasist_Optima.Models;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.SellAsistModels;
using Microsoft.EntityFrameworkCore;

namespace Sellasist_Optima.Pages
{
    public class StatusModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly SellAsistContext _context;
        private readonly ConfigurationContext _context;
        public List<Status> Status { get; set; }

        //public StatusModel(IHttpClientFactory httpClientFactory, SellAsistContext context)
        public StatusModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync(); // Pobranie pierwszej konfiguracji API
            if (apiInfo != null)
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage response = await client.GetAsync("/api/v1/statuses");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Status = JsonConvert.DeserializeObject<List<Status>>(jsonResponse);
                }
            }
        }
    }
}


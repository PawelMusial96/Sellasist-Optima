using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.SellAsistModels;

namespace Sellasist_Optima.Pages.SellAsist
{
    public class AtrybutyModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SellAsistContext _context;
        public List<Atrybuty> Atrybuty { get; set; }

        public AtrybutyModel(IHttpClientFactory httpClientFactory, SellAsistContext context)
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
                client.BaseAddress = new Uri(apiInfo.API);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage response = await client.GetAsync("/api/v1/attributes_groups");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Atrybuty = JsonConvert.DeserializeObject<List<Atrybuty>>(jsonResponse);
                }
            }
        }
    }
}

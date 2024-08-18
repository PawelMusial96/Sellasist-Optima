using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.SellAsistModels;
using System.Net.Http.Headers;
using System.Text;

namespace Sellasist_Optima.Pages.SellAsist
{
    public class AtrybutyModel : PageModel
    {

        [BindProperty]
        public string AttributeGroupName { get; set; }
        public List<AtrybutyGrupa> AtrybutyGrupa { get; set; }
        public List<Atrybuty> Atrybuty { get; set; }
        public string ErrorMessage { get; set; }


        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SellAsistContext _context;
        public AtrybutyModel(IHttpClientFactory httpClientFactory, SellAsistContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }


        public async Task OnGetAsync()
        {
            await LoadAttributesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input.";
                return Page();
            }

            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo != null)
            {
                try
                {
                    HttpClient client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(apiInfo.API);
                    client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var newAttributeGroup = new { name = AttributeGroupName };
                    var content = new StringContent(JsonConvert.SerializeObject(newAttributeGroup), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("/api/v1/attributes_groups", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage();
                    }
                    else
                    {
                        ErrorMessage = $"Error: {response.ReasonPhrase}";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "API configuration not found.";
            }

            await LoadAttributesAsync();

            return Page();
        }

        private async Task LoadAttributesAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo != null)
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.API);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage responseatrybutygrupa = await client.GetAsync("/api/v1/attributes_groups");
                if (responseatrybutygrupa.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseatrybutygrupa.Content.ReadAsStringAsync();
                    AtrybutyGrupa = JsonConvert.DeserializeObject<List<AtrybutyGrupa>>(jsonResponse);
                }

                HttpResponseMessage responseatrybuty = await client.GetAsync("/api/v1/attributes");
                if (responseatrybuty.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseatrybuty.Content.ReadAsStringAsync();
                    Atrybuty = JsonConvert.DeserializeObject<List<Atrybuty>>(jsonResponse);
                }
            }
        }
    }  
}

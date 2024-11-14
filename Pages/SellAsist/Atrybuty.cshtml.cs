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

        public Dictionary<int, List<AtrybutyGrupy>> AtrybutyByGroup { get; set; } = new Dictionary<int, List<AtrybutyGrupy>>();
        public string ErrorMessage { get; set; }


        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationContext _context;
        public AtrybutyModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
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
            if (apiInfo == null)
            {
                ErrorMessage = "API configuration not found.";
                return Page();
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
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

            await LoadAttributesAsync();

            return Page();
        }

        private async Task LoadAttributesAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo == null)
            {
                ErrorMessage = "API configuration not found.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiInfo.ShopName);
            client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

            HttpResponseMessage responseatrybutygrupa = await client.GetAsync("/api/v1/attributes_groups");
            if (responseatrybutygrupa.IsSuccessStatusCode)
            {
                var jsonResponse = await responseatrybutygrupa.Content.ReadAsStringAsync();
                AtrybutyGrupa = JsonConvert.DeserializeObject<List<AtrybutyGrupa>>(jsonResponse);

                foreach (var group in AtrybutyGrupa)
                {
                    var responseAttributes = await client.GetAsync($"/api/v1/attributes_groups/{group.Id}");
                    if (responseAttributes.IsSuccessStatusCode)
                    {
                        var jsonAttributesResponse = await responseAttributes.Content.ReadAsStringAsync();
                        var attributesGroup = JsonConvert.DeserializeObject<AtrybutyGrupa>(jsonAttributesResponse);
                        AtrybutyByGroup[group.Id] = attributesGroup.Attributes;
                    }
                    else
                    {
                        AtrybutyByGroup[group.Id] = new List<AtrybutyGrupy>();
                    }
                }
            }
        }
    }
}

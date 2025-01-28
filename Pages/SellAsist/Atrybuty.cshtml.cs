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
                ErrorMessage = "Konfiguracja API nie znaleziona.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiInfo.ShopName);
            client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

            HttpResponseMessage responseGroupList = await client.GetAsync("/api/v1/attributes_groups");
            if (responseGroupList.IsSuccessStatusCode)
            {
                var jsonResponse = await responseGroupList.Content.ReadAsStringAsync();
                var groupList = JsonConvert.DeserializeObject<List<AtrybutyGrupa>>(jsonResponse);

                AtrybutyGrupa = new List<AtrybutyGrupa>();

                foreach (var group in groupList)
                {
                    var responseGroup = await client.GetAsync($"/api/v1/attributes_groups/{group.Id}");
                    if (responseGroup.IsSuccessStatusCode)
                    {
                        var jsonGroupResponse = await responseGroup.Content.ReadAsStringAsync();
                        var groupWithAttributes = JsonConvert.DeserializeObject<AtrybutyGrupa>(jsonGroupResponse);
                        AtrybutyGrupa.Add(groupWithAttributes);
                    }
                    else
                    {
                        group.Attributes = new List<AtrybutyGrupy>();
                        AtrybutyGrupa.Add(group);
                    }
                }
            }
            else
            {
                ErrorMessage = $"B³¹d pobierania grup atrybutów: {responseGroupList.ReasonPhrase}";
            }
        }
    }
}

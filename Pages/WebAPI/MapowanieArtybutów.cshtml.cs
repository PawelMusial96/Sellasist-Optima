using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.Mapping;
using Sellasist_Optima.Models;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sellasist_Optima.Pages.WebAPI
{
    public class MapowanieArtybutówModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationContext _context;

        public List<DocumentAttributesModels> WebApiAttributes { get; set; } = new List<DocumentAttributesModels>();
        public List<Atrybuty> SellAsistAttributes { get; set; } = new List<Atrybuty>();
        public List<AttributeMappingModels> AttributeMappings { get; set; }
        public string ErrorMessage { get; set; }

        public MapowanieArtybutówModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            await LoadAttributesAsync();
        }

        public async Task<IActionResult> OnPostAsync(int WebApiAttributeId, int SellAsistAttributeId)
        {
            try
            {
                var existingMapping = await _context.AttributeMappings
                    .FirstOrDefaultAsync(m => m.WebApiAttributeId == WebApiAttributeId);

                if (existingMapping != null)
                {
                    ErrorMessage = "Mapowanie dla tego atrybutu z WebAPI już istnieje.";
                }
                else
                {
                    var mapping = new AttributeMappingModels
                    {
                        WebApiAttributeId = WebApiAttributeId,
                        SellAsistAttributeId = SellAsistAttributeId
                    };
                    _context.AttributeMappings.Add(mapping);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }

            await LoadAttributesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteMappingAsync(int MappingId)
        {
            try
            {
                var mapping = await _context.AttributeMappings.FindAsync(MappingId);
                if (mapping != null)
                {
                    _context.AttributeMappings.Remove(mapping);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }

            await LoadAttributesAsync();

            return Page();
        }

        private async Task LoadAttributesAsync()
        {
            await LoadWebApiAttributesAsync();
            await LoadSellAsistAttributesAsync();
            AttributeMappings = await _context.AttributeMappings.ToListAsync();
        }

        private async Task LoadWebApiAttributesAsync()
        {
            var webapiInfo = await _context.WebApiClient.FirstOrDefaultAsync();
            if (webapiInfo == null)
            {
                ErrorMessage = "API configuration not found";
                return;
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                var baseAddress = webapiInfo.Localhost.StartsWith("http") ? webapiInfo.Localhost : "http://" + webapiInfo.Localhost;
                client.BaseAddress = new Uri(baseAddress);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", webapiInfo.TokenAPI);

                // pobiera atrybuty type=1
                HttpResponseMessage response = await client.GetAsync("/api/AttributeDefinitions?type=1");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var allAttributes = JsonConvert.DeserializeObject<List<DocumentAttributesModels>>(jsonResponse);

                    WebApiAttributes = allAttributes.Where(attr => attr.Type == 1).ToList();
                }
                else
                {
                    ErrorMessage = $"Error fetching attributes from WebAPI: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while fetching WebAPI attributes: {ex.Message}";
            }
        }

        private async Task LoadSellAsistAttributesAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo == null)
            {
                ErrorMessage = "API configuration not found.";
                return;
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage response = await client.GetAsync("/api/v1/attributes");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    SellAsistAttributes = JsonConvert.DeserializeObject<List<Atrybuty>>(jsonResponse);
                }
                else
                {
                    ErrorMessage = $"Error fetching attributes from Sellasist API: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while fetching Sellasist attributes: {ex.Message}";
            }
        }

        public string GetWebApiAttributeName(int id)
        {
            var attr = WebApiAttributes.FirstOrDefault(a => a.Id == id);
            return attr != null ? attr.Name : "Unknown";
        }

        public string GetSellAsistAttributeName(int id)
        {
            var attr = SellAsistAttributes.FirstOrDefault(a => a.Id == id);
            return attr != null ? attr.Name : "Unknown";
        }
    }
}
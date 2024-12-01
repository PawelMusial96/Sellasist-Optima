using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
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
    public class DocumentAttributesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationContext _context;

        public List<DocumentAttributes> Attributes { get; set; }
        public string ErrorMessage { get; set; }

        public DocumentAttributesModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            await LoadAttributesAsync();
        }

        private async Task LoadAttributesAsync()
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

                HttpResponseMessage response = await client.GetAsync("/api/AttributeDefinitions?type=4");
                //HttpResponseMessage response = await client.GetAsync("/api/AttributeDefinitions");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Attributes = JsonConvert.DeserializeObject<List<DocumentAttributes>>(jsonResponse);
                }
                else
                {
                    ErrorMessage = $"Error fetching attributes: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }
    }
}
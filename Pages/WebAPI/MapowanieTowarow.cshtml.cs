using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sellasist_Optima.BazyDanych;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sellasist_Optima.Pages.WebAPI
{
    public class MapowanieTowarowModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationContext _context;

        public List<SellAsistProduct> SellAsistProducts { get; set; } = new List<SellAsistProduct>();
        public List<WebApiProduct> WebApiProducts { get; set; } = new List<WebApiProduct>();
        public List<ProductMapping> ProductMappings { get; set; }
        public string ErrorMessage { get; set; }

        public MapowanieTowarowModel(IHttpClientFactory httpClientFactory, ConfigurationContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            await LoadProductsAsync();
        }

        public async Task<IActionResult> OnPostAsync(int SellAsistProductId, int WebApiProductId)
        {
            try
            {
                // Sprawdzamy, czy istnieje ju¿ mapowanie dla podanego produktu
                var existingMapping = await _context.ProductMappings
                    .FirstOrDefaultAsync(m => m.SellAsistProductId == SellAsistProductId);

                if (existingMapping != null)
                {
                    ErrorMessage = "Mapowanie dla tego towaru z SellAsist ju¿ istnieje.";
                }
                else
                {
                    // Tworzymy nowe mapowanie bez sprawdzania tabel SQL
                    var mapping = new ProductMapping
                    {
                        SellAsistProductId = SellAsistProductId,
                        WebApiProductId = WebApiProductId
                    };
                    _context.ProductMappings.Add(mapping);
                    await _context.SaveChangesAsync(); // Zapisujemy mapowanie
                }
            }
            catch (Exception ex)
            {
                // Obs³uga b³êdu i wyœwietlenie szczegó³ów
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    ErrorMessage += innerException.Message + " ";
                    innerException = innerException.InnerException;
                }
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ex.Message;
                }
            }

            // Za³aduj produkty ponownie, aby odœwie¿yæ widok
            await LoadProductsAsync();

            return Page();
        }
        //public async Task<IActionResult> OnPostAsync(int SellAsistProductId, int WebApiProductId)
        //{
        //    try
        //    {
        //        var existingMapping = await _context.ProductMappings
        //            .FirstOrDefaultAsync(m => m.SellAsistProductId == SellAsistProductId);

        //        if (existingMapping != null)
        //        {
        //            ErrorMessage = "Mapowanie dla tego towaru z SellAsist ju¿ istnieje.";
        //        }
        //        else
        //        {
        //            var mapping = new ProductMapping
        //            {
        //                SellAsistProductId = SellAsistProductId,
        //                WebApiProductId = WebApiProductId
        //            };
        //            _context.ProductMappings.Add(mapping);
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var innerException = ex.InnerException;
        //        while (innerException != null)
        //        {
        //            ErrorMessage += innerException.Message + " ";
        //            innerException = innerException.InnerException;
        //        }
        //        if (string.IsNullOrEmpty(ErrorMessage))
        //        {
        //            ErrorMessage = ex.Message;
        //        }
        //    }

        //    await LoadProductsAsync();

        //    return Page();
        //}

        public async Task<IActionResult> OnPostDeleteMappingAsync(int MappingId)
        {
            try
            {
                var mapping = await _context.ProductMappings.FindAsync(MappingId);
                if (mapping != null)
                {
                    _context.ProductMappings.Remove(mapping);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wyst¹pi³ b³¹d: {ex.Message}";
            }

            await LoadProductsAsync();

            return Page();
        }

        private async Task LoadProductsAsync()
        {
            await LoadSellAsistProductsAsync();
            await LoadWebApiProductsAsync();
            ProductMappings = await _context.ProductMappings.ToListAsync(); // Dodano liniê
        }

        private async Task LoadSellAsistProductsAsync()
        {
            var apiInfo = await _context.SellAsistAPI.FirstOrDefaultAsync();
            if (apiInfo == null)
            {
                ErrorMessage = "Konfiguracja SellAsist API nie znaleziona.";
                return;
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(apiInfo.ShopName);
                client.DefaultRequestHeaders.Add("apiKey", apiInfo.KeyAPI);

                HttpResponseMessage response = await client.GetAsync("/api/v1/products");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    SellAsistProducts = JsonConvert.DeserializeObject<List<SellAsistProduct>>(jsonResponse);
                }
                else
                {
                    ErrorMessage = $"B³¹d pobierania towarów z SellAsist API: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wyst¹pi³ b³¹d podczas pobierania towarów z SellAsist: {ex.Message}";
            }
        }

        private async Task LoadWebApiProductsAsync()
        {
            var webapiInfo = await _context.WebApiClient.FirstOrDefaultAsync();
            if (webapiInfo == null)
            {
                ErrorMessage = "Konfiguracja WebAPI nie znaleziona.";
                return;
            }

            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                var baseAddress = webapiInfo.Localhost.StartsWith("http") ? webapiInfo.Localhost : "http://" + webapiInfo.Localhost;
                client.BaseAddress = new Uri(baseAddress);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", webapiInfo.TokenAPI);

                HttpResponseMessage response = await client.GetAsync("/api/Items");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    WebApiProducts = JsonConvert.DeserializeObject<List<WebApiProduct>>(jsonResponse);
                }
                else
                {
                    ErrorMessage = $"B³¹d pobierania towarów z WebAPI: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wyst¹pi³ b³¹d podczas pobierania towarów z WebAPI: {ex.Message}";
            }
        }

        public string GetSellAsistProductName(int id)
        {
            var product = SellAsistProducts.FirstOrDefault(p => p.Id == id);
            return product != null ? $"{product.Name} ({product.EAN})" : "Nieznany";
        }

        public string GetWebApiProductName(int id)
        {
            var product = WebApiProducts.FirstOrDefault(p => p.Id == id);
            return product != null ? $"{product.Name} ({product.barcode})" : "Nieznany";
        }
    }
}
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
                // Pobieramy istniej¹cy rekord z tabeli WebApiClient - zak³adamy ¿e jest tylko jeden
                var existingWebApiClient = await _context.WebApiClient.FirstOrDefaultAsync();

                if (existingWebApiClient == null)
                {
                    // Je¿eli brak jakiegokolwiek rekordu, tworzymy nowy
                    _context.WebApiClient.Add(WebApiClient);
                    int saved = await _context.SaveChangesAsync();
                    Message = saved > 0
                        ? "Nowa konfiguracja Web API zosta³a zapisana."
                        : "Nie uda³o siê zapisaæ konfiguracji Web API.";
                }
                else
                {
                    // Rekord istnieje, aktualizujemy go nowymi danymi
                    existingWebApiClient.Username = WebApiClient.Username;
                    existingWebApiClient.Password = WebApiClient.Password;
                    existingWebApiClient.Grant_type = WebApiClient.Grant_type;
                    existingWebApiClient.Localhost = WebApiClient.Localhost;
                    existingWebApiClient.TokenAPI = WebApiClient.TokenAPI;

                    int saved = await _context.SaveChangesAsync();
                    Message = saved > 0
                        ? "Konfiguracja Web API zosta³a zaktualizowana."
                        : "Nie uda³o siê zaktualizowaæ konfiguracji Web API.";
                }
            }
            catch (Exception ex)
            {
                Message = $"B³¹d podczas zapisywania konfiguracji Web API: {ex.Message}";
            }

            AllWebApiClient = await _context.WebApiClient.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostConnectAPIAsync()
        {
            var client = _clientFactory.CreateClient("SellAsistApiClient");

            try
            {
                client.BaseAddress = new Uri(SellAsistAPI.ShopName);
            }
            catch (UriFormatException)
            {
                ModelState.AddModelError(string.Empty, "Nieprawid³owy adres sklepu");
                AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
                return Page();
            }

            client.DefaultRequestHeaders.Add("apiKey", SellAsistAPI.KeyAPI);

            HttpResponseMessage response = await client.GetAsync("endpoint");

            var existingSellAsistAPI = await _context.SellAsistAPI.FirstOrDefaultAsync();

            if (existingSellAsistAPI == null)
            {
                // Brak wpisu w bazie, tworzymy nowy
                _context.SellAsistAPI.Add(SellAsistAPI);
                int saved = await _context.SaveChangesAsync();
                Message = saved > 0
                    ? "Nowa konfiguracja Sellasist API zosta³a zapisana."
                    : "Nie uda³o siê zapisaæ konfiguracji Sellasist API.";
            }
            else
            {
                // Istnieje ju¿ jeden rekord, aktualizujemy go nowymi danymi
                existingSellAsistAPI.ShopName = SellAsistAPI.ShopName;
                existingSellAsistAPI.KeyAPI = SellAsistAPI.KeyAPI;

                int saved = await _context.SaveChangesAsync();
                Message = saved > 0
                    ? "Konfiguracja Sellasist API zosta³a zaktualizowana."
                    : "Nie uda³o siê zaktualizowaæ konfiguracji Sellasist API.";
            }

            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();

            return Page();
        }    
    }
}
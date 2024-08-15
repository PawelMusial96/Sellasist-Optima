
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using Sellasist_Optima.Models;
using Sellasist_Optima.BazyDanych;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;  // Ensure this namespace is included for List<>

namespace Sellasist_Optima.Pages
{
    public class SellAsistAPIModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SellAsistContext _context;

        [BindProperty]
        public SellAsistAPI SellAsistAPI { get; set; }
        public List<SellAsistAPI> AllSellAsistAPI { get; set; } = new List<SellAsistAPI>();

        public SellAsistAPIModel(IHttpClientFactory clientFactory, SellAsistContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
        }

        public async Task<IActionResult> OnPostConnectAsync()
        {
            var client = _clientFactory.CreateClient("SellAsistApiClient");
            client.BaseAddress = new Uri(SellAsistAPI.API);
            client.DefaultRequestHeaders.Add("apiKey", SellAsistAPI.KeyAPI);

            HttpResponseMessage response = await client.GetAsync("endpoint");
            bool exists = await _context.SellAsistAPI.AnyAsync(api => api.API == SellAsistAPI.API && api.KeyAPI == SellAsistAPI.KeyAPI);

            if (!exists)
            {
                _context.SellAsistAPI.Add(SellAsistAPI);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    ViewData["Result"] = "Zapis poprawny";
                }
                else
                {
                    ViewData["Result"] = "Zapis nie powiód³ siê";
                }
            }
            else
            {
                ViewData["Result"] = "Takie API ju¿ istnieje w systemie.";
            }

            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();

            return Page();
        }
    }
}

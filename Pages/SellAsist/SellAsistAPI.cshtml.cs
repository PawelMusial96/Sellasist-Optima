
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using Sellasist_Optima.Models;
using Sellasist_Optima.BazyDanych;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;  // Ensure this namespace is included for List<>

namespace Sellasist_Optima.Pages
{

    public class SellAsistAPIModel : PageModel
    {
        private readonly SellAsistContext _context;

        [BindProperty]
        public SellAsistAPI SellAsistAPI { get; set; }

        public List<SellAsistAPI> AllSellAsistAPI { get; set; } = new List<SellAsistAPI>();

        public bool IsConnected { get; set; } = false;

        public SellAsistAPIModel(SellAsistContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            // Fetch all API connections from the database
            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
        }

        public async Task<IActionResult> OnPostConnectAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Result"] = "Failed to connect. Please check the input.";
                return Page();
            }

            // Check if the API connection already exists
            bool exists = await _context.SellAsistAPI.AnyAsync(api => api.API == SellAsistAPI.API && api.KeyAPI == SellAsistAPI.KeyAPI);

            if (!exists)
            {
                _context.SellAsistAPI.Add(SellAsistAPI);
                await _context.SaveChangesAsync();

                ViewData["Result"] = "Successfully connected!";
            }
            else
            {
                ViewData["Result"] = "This API connection already exists.";
            }

            // Update the list after inserting the new connection
            AllSellAsistAPI = await _context.SellAsistAPI.ToListAsync();
            IsConnected = true;

            return Page();
        }
    }
}

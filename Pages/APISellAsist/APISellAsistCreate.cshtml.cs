using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Sellasist_Optima.Models;
using Sellasist_Optima.Areas.Identity.Data;
using Microsoft.Extensions.Logging;

namespace Sellasist_Optima.Pages.APISellAsist
{
    public class APISellAsistCreateModel : PageModel
    {
        private readonly Sellasist_OptimaContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<APISellAsistCreateModel> _logger;

        public APISellAsistCreateModel(ILogger<APISellAsistCreateModel> logger, Sellasist_OptimaContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SellAsistAPI SellAsistAPI { get; set; } = new SellAsistAPI();

        public async Task<IActionResult> OnPostAsync()
        {

            _logger.LogInformation("Próba zapisu danych...");

            if (!ModelState.IsValid)
            {
                // Jeśli model nie jest poprawny, zwracamy stronę z błędami walidacji
                return Page();
            }

            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "Nie zalogowano użytkownika.");
                return Page();
            }

            SellAsistAPI.UserId = currentUser.Id;

            try
            {
                _context.SellAsistAPI.Add(SellAsistAPI);
                await _context.SaveChangesAsync();
                return RedirectToPage("./APISellAsistList");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                ModelState.AddModelError("", "Nie można zapisać zmian. Spróbuj ponownie.");
                return Page();
            }
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    try
        //    {
        //        var currentUser = await _userManager.GetUserAsync(User);
        //        if (currentUser != null)
        //        {
        //            SellAsistAPI.UserId = currentUser.Id;
        //        }

        //        _context.sellasistapi.Add(SellAsistAPI);
        //        await _context.SaveChangesAsync();
        //        return RedirectToPage("./APISellAsistList");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        ModelState.AddModelError("", "Nie można zapisać zmian. Spróbuj ponownie.");
        //        return Page();
        //    }
        //}
    }
}

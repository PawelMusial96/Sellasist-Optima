using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sellasist_Optima.Pages
{
    public class ConnectToWebAPIModel : PageModel
    {

        [BindProperty]
        public string KeyApi { get; set; }
        [BindProperty]
        public string Login { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }
        public void OnGet()
        {
        }
    }
}

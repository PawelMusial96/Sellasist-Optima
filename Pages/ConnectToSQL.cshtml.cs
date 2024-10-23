using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace YourAppNamespace.Pages
{
    public class ConnectToSQLModel : PageModel
    {
        [BindProperty]
        public string ServerName { get; set; }
        [BindProperty]
        public string DatabaseName { get; set; }
        [BindProperty]
        public string Login { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // Load the page
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName) || string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                Message = "Please fill in all the required fields.";
                return Page();
            }

            string connectionString = $"Server={ServerName};Database={DatabaseName};User Id={Login};Password={Password};";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Message = "Connection Successful!";
                }
            }
            catch (SqlException ex)
            {
                Message = $"Connection Failed: {ex.Message}";
            }

            return Page();
        }
    }
}

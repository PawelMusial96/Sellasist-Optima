using Microsoft.Build.Framework;
using System.ComponentModel;

namespace Sellasist_Optima.WebApiModels
{
    public class WebApiClient
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        public string KeyWebAPI { get; set; }
    }
}

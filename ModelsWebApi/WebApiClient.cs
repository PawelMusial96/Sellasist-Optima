using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;


namespace Sellasist_Optima.WebApiModels
{
    public class WebApiClient
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string Grant_type { get; set; }

        [Required]
        [MaxLength (255)]
        public string Localhost { get; set; }

        [Required]
        [MaxLength(400)]
        public string TokenAPI { get; set; }

        [MaxLength(255)]
        public string CompanyName { get; set; }

        [MaxLength(255)]
        public string DatabaseName { get; set; }
    }
}

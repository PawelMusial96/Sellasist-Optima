using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Sellasist_Optima.Models
{
    public class SellAsistAPI
    {
        public int Id { get; set; }

        [Required]
        public string API { get; set; }

        [Required]
        public string KeyAPI { get; set; }

        //[Required]
        //public string UserId { get; set; } // ID zalogowanego użytkownika

    }

}

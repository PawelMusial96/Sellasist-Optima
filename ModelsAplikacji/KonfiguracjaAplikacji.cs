using System.ComponentModel.DataAnnotations;

namespace Sellasist_Optima.ModelsAplikacji
{
    public class KonfiguracjaAplikacji
    {
        [Key]
        public int Id { get; set; }
        public string? PayerCodeName { get; set; }
    }
}

using Sellasist_Optima.SellAsistModels;

namespace Sellasist_Optima.WebApiModels
{
    public class ProductMapping
    {
        public int Id { get; set; }
        public int SellAsistProductId { get; set; }
        public int WebApiProductId { get; set; }
        public SellAsistProduct SellAsistProduct { get; set; }
        public WebApiProduct WebApiProduct { get; set; }
    }
}

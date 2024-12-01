namespace Sellasist_Optima.ModelsWebApi.Products
{
    public class Price
    {
        public int Id { get; set; }
        public string OptimaItemId { get; set; }
        public string ItemBarcode { get; set; }
        public int? Number { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public string VatRate { get; set; }
        public string Unit { get; set; }
        public string ItemCode { get; set; }
        public string Error { get; set; }
    }
}

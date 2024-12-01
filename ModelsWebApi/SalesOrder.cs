namespace Sellasist_Optima.WebApiModels
{
    public class SalesOrder
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<SalesOrderItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderDate { get; set; }
    }

    public class SalesOrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}


using Newtonsoft.Json;

namespace Sellasist_Optima.WebApiModels
{
    public class Order
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("number")]
        public string number { get; set; }
        [JsonProperty("date")]
        public string date { get; set; }
        [JsonProperty("total_price")]
        public decimal total_price { get; set; }
        [JsonProperty("customer")]
        public Customer customer { get; set; }
        [JsonProperty("cart_items")]
        public List<CartItem> cart_items { get; set; }

        public int status_id { get; set; }
    }

    public class Customer
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
    }

    public class CartItem
    {
        [JsonProperty("product_id")]
        public int product_id { get; set; }
        [JsonProperty("product_name")]
        public string product_name { get; set; }
        [JsonProperty("quantity")]
        public int quantity { get; set; }
        [JsonProperty("price")]
        public decimal price { get; set; }
    }
}

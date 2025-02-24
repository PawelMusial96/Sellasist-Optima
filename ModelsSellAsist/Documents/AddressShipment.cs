using Newtonsoft.Json;

namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class AddressShipment
    {
        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("home_number")]
        public string HomeNumber { get; set; }

        [JsonProperty("flat_number")]
        public string FlatNumber { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("company_nip")]
        public string CompanyNip { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }
    }
}

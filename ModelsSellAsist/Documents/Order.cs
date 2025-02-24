using Newtonsoft.Json;
using Sellasist_Optima.SellAsistModels;

namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; } // Unikalny identyfikator zamówienia

        [JsonProperty("date")]
        public DateTime Date { get; set; } // Data utworzenia zamówienia

        [JsonProperty("status")]
        public Status Status { get; set; } // Aktualny status zamówienia

        [JsonProperty("shipment")]
        public Shipment Shipment { get; set; } // Informacje o wysyłce

        [JsonProperty("payment")]
        public Payment Payment { get; set; } // Informacje o płatności

        [JsonProperty("external_data")]
        public ExternalData ExternalData { get; set; } // Dane zewnętrzne (z innego systemu)

        [JsonProperty("source")]
        public string Source { get; set; } // Źródło zamówienia

        [JsonProperty("shop")]
        public string Shop { get; set; } // Nazwa sklepu

        [JsonProperty("deadline")]
        public DateTime Deadline { get; set; } // Termin realizacji zamówienia

        [JsonProperty("important")]
        public bool Important { get; set; } // Czy zamówienie jest oznaczone jako ważne

        [JsonProperty("placeholder")]
        public int Placeholder { get; set; } // Pole pomocnicze

        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; } // Numer śledzenia przesyłki

        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; } // Numer dokumentu (np. zamówienia)

        [JsonProperty("invoice")]
        public int Invoice { get; set; } // 1 = faktura, 0 = brak

        [JsonProperty("email")]
        public string Email { get; set; } // Adres e-mail klienta

        [JsonProperty("total")]
        public decimal Total { get; set; } // Suma (wartość całkowita) zamówienia

        [JsonProperty("comment")]
        public string Comment { get; set; } // Dodatkowy komentarz do zamówienia

        public string CodCustomer {  get; set; } // Kod klienta oraz nazwa

        // Najważniejsza zmiana: mapujemy "bill_address" na właściwość BillAddress
        [JsonProperty("bill_address")]
        public AddressBill BillAddress { get; set; } // Adres rozliczeniowy

        // Mapa "shipment_address" => ShipmentAddress
        [JsonProperty("shipment_address")]
        public AddressShipment ShipmentAddress { get; set; } // Adres wysyłki

        [JsonProperty("pickup_point")]
        public PickupPoint PickupPoint { get; set; } // Punkt odbioru (jeśli dotyczy)

        [JsonProperty("carts")]
        public List<CartItem> Carts { get; set; } // Lista produktów w koszyku

        [JsonProperty("payments")]
        public List<PaymentDetail> Payments { get; set; } // Szczegółowe informacje o płatnościach

        [JsonProperty("shipments")]
        public List<ShipmentDetail> Shipments { get; set; } // Szczegółowe informacje o wysyłkach

        [JsonProperty("additional_fields")]
        public List<AdditionalField> AdditionalFields { get; set; } // Dodatkowe pola (niestandardowe)
    }
}

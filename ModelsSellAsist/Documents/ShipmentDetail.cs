namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class ShipmentDetail
    {
        public int OrderShipmentId { get; set; } // Identyfikator wysyłki w zamówieniu
        public string Service { get; set; } // Usługa wysyłki (np. "Kurier", "InPost")
        public string TrackingNumber { get; set; } // Numer śledzenia przesyłki
    }
}

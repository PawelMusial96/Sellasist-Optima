namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class Shipment
    {
        public int Id { get; set; } // Identyfikator metody wysyłki
        public string Name { get; set; } // Nazwa metody wysyłki (np. "Kurier", "InPost")
        public string Total { get; set; } // Koszt wysyłki w postaci tekstu (np. "12.50")

    }
}

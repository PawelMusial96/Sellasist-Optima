namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class ExternalData
    {
        public string ExternalId { get; set; } // Zewnętrzny identyfikator zamówienia
        public string ExternalLogin { get; set; } // Zewnętrzny login użytkownika
        public int ExternalUserId { get; set; } // Identyfikator użytkownika w innym systemie
        public int ExternalAccount { get; set; } // Numer konta w systemie zewnętrznym
        public string ExternalSite { get; set; } // Nazwa serwisu (np. "Allegro")
        public string ExternalPaymentName { get; set; } // Nazwa płatności w systemie zewnętrznym
        public string ExternalShipmentName { get; set; } // Nazwa wysyłki w systemie zewnętrznym
        public string ExternalType { get; set; } // Rodzaj zewnętrznego zamówienia (np. "aukcja")

    }
}

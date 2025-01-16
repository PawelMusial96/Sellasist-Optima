namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class PaymentDetail
    {
        public int PaymentId { get; set; } // Identyfikator płatności
        public DateTime Date { get; set; } // Data zarejestrowania płatności
        public string PaymentData { get; set; } // Dane o płatności (np. nr transakcji)
        public decimal Amount { get; set; } // Kwota płatności
        public string Currency { get; set; } // Waluta płatności (np. "PLN")
        public int Status { get; set; } // Status płatności (np. 1 = zakończona)
        public string Name { get; set; } // Nazwa płatności (wewnętrzna)
    }
}

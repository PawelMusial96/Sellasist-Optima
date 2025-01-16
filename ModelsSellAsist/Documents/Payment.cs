namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class Payment
    {
        public int Id { get; set; } // Identyfikator płatności
        public string Name { get; set; } // Nazwa metody płatności (np. "Przelew")
        public decimal? Paid { get; set; } // Kwota, która została zapłacona
        public DateTime? PaidDate { get; set; } // Data opłacenia (jeśli opłacono)
        public int Cod { get; set; } // Informacja o pobraniu (np. 1 = pobranie)
        public string Status { get; set; } // Aktualny status płatności
        public string Currency { get; set; } // Waluta płatności (np. "PLN")
        public decimal Tax { get; set; } // Kwota podatku (VAT)
    }
}

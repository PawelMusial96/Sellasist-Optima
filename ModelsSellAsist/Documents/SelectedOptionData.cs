namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class SelectedOptionData
    {
        public string Name { get; set; } // Nazwa opcji (np. "Kolor")
        public string Prop { get; set; } // Wartość wybranej opcji (np. "Czerwony")
        public decimal Price { get; set; } // Dodatkowy koszt wybranej opcji
        public int OptionId { get; set; } // Identyfikator opcji w systemie
        public int VariantId { get; set; } // Identyfikator wariantu produktu w systemie
    }
}

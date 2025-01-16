namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class CartItem
    {
        public int Id { get; set; } // Identyfikator elementu koszyka
        public int? ProductId { get; set; } // Identyfikator produktu (jeśli istnieje)
        public int? VariantId { get; set; } // Identyfikator wariantu produktu (jeśli istnieje)
        public string Name { get; set; } // Nazwa produktu
        public string Image { get; set; } // Link do zdjęcia produktu
        public string ImageThumb { get; set; } // Link do miniatury zdjęcia
        public decimal Quantity { get; set; } // Ilość zamówionego produktu
        public decimal Price { get; set; } // Cena jednostkowa produktu
        public string Weight { get; set; } // Waga produktu (tekstowo, np. "1kg")
        public string Ean { get; set; } // Kod EAN produktu
        public string AdditionalInformation { get; set; } // Dodatkowe informacje
        public string TaxRate { get; set; } // Stawka podatku (np. "23%")
        public string SelectedOptions { get; set; } // Wybrane opcje w formie tekstu
        public List<SelectedOptionData> SelectedOptionsData { get; set; } // Detale wybranych opcji
        public string Symbol { get; set; } // Symbol / SKU produktu
        public string CatalogNumber { get; set; } // Numer katalogowy produktu
        public string ExternalOfferId { get; set; } // Identyfikator oferty w systemie zewnętrznym
        public decimal PriceBuy { get; set; } // Cena zakupu (koszt dla sprzedawcy)
    }
}

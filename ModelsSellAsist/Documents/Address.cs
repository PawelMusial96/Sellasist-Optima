namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class Address
    {
        public string Name { get; set; } // Imię odbiorcy
        public string Surname { get; set; } // Nazwisko odbiorcy
        public string Street { get; set; } // Ulica
        public string HomeNumber { get; set; } // Numer domu
        public string FlatNumber { get; set; } // Numer mieszkania
        public string Description { get; set; } // Opis adresu (np. brama, piętro)
        public string Postcode { get; set; } // Kod pocztowy
        public string City { get; set; } // Miasto
        public string State { get; set; } // Województwo / stan
        public string Phone { get; set; } // Numer telefonu
        public string CompanyName { get; set; } // Nazwa firmy (jeśli dotyczy)
        public string CompanyNip { get; set; } // NIP firmy (jeśli dotyczy)
        public Country Country { get; set; } // Kraj (nazwa i kod)
    }
}

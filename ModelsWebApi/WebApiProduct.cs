using Sellasist_Optima.ModelsWebApi.Products;
using Attribute = Sellasist_Optima.ModelsWebApi.Products.Attribute;

namespace Sellasist_Optima.WebApiModels
{
    public class WebApiProduct
    {

        // Unikalny identyfikator przedmiotu w Optima
        public int Id { get; set; }

        // Typ przedmiotu: 0 - usługa, 1 - przedmiot
        public int Type { get; set; }

        // Stan aktywności przedmiotu: 0 - aktywny, 1 - nieaktywny
        public int? Inactive { get; set; }

        // Unikalny kod przedmiotu w Optima
        public string Code { get; set; } // wymagany 1-50

        // Nazwa przedmiotu
        public string Name { get; set; } // wymagany 1-255

        // Kod producenta
        public string ManufacturerCode { get; set; } // opcjonalny 0-50

        // Stawka VAT
        public decimal VatRate { get; set; }

        // Flaga stawki VAT
        public int? VatRateFlag { get; set; } // 0 - VAT, 1 - zwolniony, 2 - opodatkowany, 4 - brak VAT

        // Jednostka miary
        public string Unit { get; set; } // wymagany 1-20

        // Kod kreskowy
        public string Barcode { get; set; } // opcjonalny 0-40

        // Opis przedmiotu
        public string Description { get; set; } // opcjonalny

        // Kod dostawcy
        public string SupplierCode { get; set; } // opcjonalny 0-50

        // Numer katalogowy
        public string CatalogNumber { get; set; } // opcjonalny 0-40

        // Wartość dla pakietu/kaucji: 0 - brak, 1 - tak
        public int? PackageDeposit { get; set; }

        // Rodzaj produktu: 0 - prosty produkt, 1 - złożony produkt
        public int Product { get; set; }

        // Grupa domyślna
        public string DefaultGroup { get; set; }

        // Kategoria sprzedaży
        public string SalesCategory { get; set; }

        // Wysokość przedmiotu
        public int? Height { get; set; }

        // Szerokość przedmiotu
        public int? Width { get; set; }

        // Długość przedmiotu
        public int? Length { get; set; }

        // Data utworzenia przedmiotu (w formacie yyyy-MM-dd)
        public string Created { get; set; }

        // Data ostatniej aktualizacji przedmiotu (w formacie yyyy-MM-dd)
        public string Updated { get; set; }

        // Lista cen przedmiotu
        public List<Price> Prices { get; set; }

        // Dane dotyczące firmy
        public CompanyData CompanyData { get; set; }

        // Lista atrybutów przedmiotu
        public List<Attribute> Attributes { get; set; }

        // Parametr związany z elementami FSPA
        public int? GettingElementsForFSPA { get; set; }
    }
}
namespace Sellasist_Optima.ModelsWebApi.Dokumenty
{
    public class DokumentRequest
    {
        public int Type { get; set; } // Typ dokumentu w systemie Optima (np. 301 - Faktura zakupu)
        public string FullNumber { get; set; } // Pełny numer dokumentu w Optima
        public string ForeignNumber { get; set; } // Numer zewnętrzny dokumentu
        public int CalculatedOn { get; set; } // 1 - Subtotal (Net), 2 - Total (Gross)
        public string PaymentMethod { get; set; } // Metoda płatności w Optima
        public string Currency { get; set; } // Waluta w systemie Optima
        public string Description { get; set; } // Opis dokumentu
        public int Status { get; set; } // Status dokumentu: 0 - potwierdzony, 1 - w edycji
        public int SourceWarehouseId { get; set; } // ID magazynu źródłowego
        public int? TargetWarehouseId { get; set; } // ID magazynu docelowego (dotyczy tylko typu 312)
        public DateTime? DocumentSaleDate { get; set; } // Data sprzedaży dokumentu
        public DateTime? DocumentIssueDate { get; set; } // Data wystawienia dokumentu
        public DateTime? DocumentPaymentDate { get; set; } // Data płatności dokumentu
        public DateTime? DocumentReservationDate { get; set; } // Data rezerwacji dokumentu
        public DateTime? DocumentReceiptDate { get; set; } // Data odbioru dokumentu
        public DateTime? DocumentReceptionDate { get; set; } // Data przyjęcia dokumentu
        public DateTime? DocumentDeliveryDate { get; set; } // Data dostawy dokumentu
        public DateTime? DocumentPurchaseDate { get; set; } // Data zakupu dokumentu
        public DateTime? DocumentReleaseDate { get; set; } // Data wydania dokumentu
        public DateTime? DocumentCorrectionDate { get; set; } // Data korekty dokumentu
        public DateTime? DocumentExchangeRateDate { get; set; } // Data kursu wymiany
        public decimal? ExchangeRate { get; set; } // Kurs wymiany waluty
        public int? ExchangeRateType { get; set; } // Typ kursu wymiany waluty
        public decimal? AmountPaid { get; set; } // Kwota zapłacona
        public decimal? AmountToPay { get; set; } // Kwota do zapłaty
        public bool CheckIfExists { get; set; } // Sprawdzanie duplikatu na podstawie numeru zewnętrznego i typu
        public string Symbol { get; set; } // Symbol dokumentu
        public string Series { get; set; } // Seria dokumentu
        public int Number { get; set; } // Numer dokumentu
        public int? State { get; set; } // Stan dokumentu (dotyczy tylko typów 308 i 309)
        public string Category { get; set; } // Kategoria dokumentu
        public string IntrastatCountryCode { get; set; } // Kod kraju dla Intrastat
        public string IntrastatTransactionCode { get; set; } // Kod transakcji dla Intrastat
        public DateTime? IntrastatExportDate { get; set; } // Data eksportu Intrastat
        public CustomerPayer Payer { get; set; } // Płatnik dokumentu
        public CustomerRecipent Recipent { get; set; } // Odbiorca dokumentu
        public CustomerPayer DefaultPayer { get; set; } // Domyślny płatnik dokumentu
        public List<Element> Elements { get; set; } // Elementy dokumentu (towary, usługi, etc.)
        public List<Attribute> Attributes { get; set; } // Atrybuty dokumentu
        public List<string> Jpkv7Codes { get; set; } // Kody JPKV7 dla dokumentu
        //public CompanyData CompanyData { get; set; } // Dane firmy (dla dokumentu)

        // Konstruktor
        public DokumentRequest()
        {
            Elements = new List<Element>();
            Attributes = new List<Attribute>();
            Jpkv7Codes = new List<string>();
        }
    }

    // Reprezentacja Płatnika, Odbiorcy, Domyślnego Płatnika
    public class CustomerPayer
    {
        public string Code { get; set; } // Kod klienta
        public string Name1 { get; set; } // Pierwsza linia nazwy klienta
        public int Type { get; set; } // Typ podmiotu (1 - klient, 2 - bank, 3 - pracownik, 4 - biuro)
        public string BankAccountNumber { get; set; } // Numer konta bankowego
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
    }

    public class CustomerRecipent
    {
        public string Code { get; set; } // Kod klienta
        public string Name1 { get; set; } // Pierwsza linia nazwy klienta
        public int Type { get; set; } // Typ podmiotu (1 - klient, 2 - bank, 3 - pracownik, 4 - biuro)
        public string BankAccountNumber { get; set; } // Numer konta bankowego
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
    }

    // Reprezentacja Elementu w dokumencie (towary, usługi)
    public class Element
    {
        public int ItemId { get; set; } // ID elementu (towaru/usługi)
        public string Code { get; set; } // Kod elementu
        public string Name { get; set; } // Nazwa elementu
        public string ManufacturerCode { get; set; } // Kod producenta
        public string Category { get; set; } // Kategoria elementu
        public decimal UnitNetPrice { get; set; } // Cena jednostkowa netto
        public decimal UnitGrossPrice { get; set; } // Cena jednostkowa brutto
        public decimal TotalNetValue { get; set; } // Całkowita wartość netto
        public decimal TotalGrossValue { get; set; } // Całkowita wartość brutto
        public decimal Quantity { get; set; } // Ilość towaru
        public decimal VatRate { get; set; } // Stawka VAT
        public int VatRateFlag { get; set; } // Flaga VAT
        public bool SetCustomValue { get; set; } // Ustawienie niestandardowej wartości
        public List<DeliveryLot> DeliveryLot { get; set; } // Partie dostawy
        public List<Attribute> Attributes { get; set; } // Atrybuty elementu

        // Konstruktor
        public Element()
        {
            DeliveryLot = new List<DeliveryLot>();
            Attributes = new List<Attribute>();
        }
    }

    // Reprezentacja Partii dostawy dla elementu
    public class DeliveryLot
    {
        public string LotNumber { get; set; } // Numer partii
        public DateTime DeliveryDate { get; set; } // Data dostawy
        public decimal Quantity { get; set; } // Ilość w partii
    }

    // Reprezentacja Atrybutu dla elementu
    public class Attribute
    {
        public string Code { get; set; } // Kod atrybutu
        public string Value { get; set; } // Wartość atrybutu
    }

    // Reprezentacja danych firmy
    //public class CompanyData
    //{
    //    public string CompanyName { get; set; } // Nazwa firmy
    //    public string DatabaseName { get; set; } // Nazwa bazy danych
    //}
}


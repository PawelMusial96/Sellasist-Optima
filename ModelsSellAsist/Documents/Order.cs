﻿namespace Sellasist_Optima.ModelsSellAsist.Documents
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Może być zmieniony w zależności od typu daty
        public Status Status { get; set; }
        public Shipment Shipment { get; set; }
        public Payment Payment { get; set; }
        public ExternalData ExternalData { get; set; }
        public string Source { get; set; }
        public string Shop { get; set; }
        public DateTime Deadline { get; set; }
        public bool Important { get; set; }
        public int Placeholder { get; set; }
        public string TrackingNumber { get; set; }
        public string DocumentNumber { get; set; }
        public int Invoice { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public string Comment { get; set; }
        public Address BillAddress { get; set; }
        public Address ShipmentAddress { get; set; }
        public PickupPoint PickupPoint { get; set; }
        public List<CartItem> Carts { get; set; }
        public List<PaymentDetail> Payments { get; set; }
        public List<ShipmentDetail> Shipments { get; set; }
        public List<AdditionalField> AdditionalFields { get; set; }
    }

    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Shipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Paid { get; set; }
        public DateTime PaidDate { get; set; }
        public bool Cod { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Tax { get; set; }
    }

    public class ExternalData
    {
        public string ExternalId { get; set; }
        public string ExternalLogin { get; set; }
        public int ExternalUserId { get; set; }
        public int ExternalAccount { get; set; }
        public string ExternalSite { get; set; }
        public string ExternalPaymentName { get; set; }
        public string ExternalShipmentName { get; set; }
        public string ExternalType { get; set; }
    }

    public class Address
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
        public string FlatNumber { get; set; }
        public string Description { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNip { get; set; }
        public Country Country { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class PickupPoint
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ImageThumb { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Weight { get; set; }
        public string Ean { get; set; }
        public string AdditionalInformation { get; set; }
        public string TaxRate { get; set; }
        public string SelectedOptions { get; set; }
        public List<SelectedOptionData> SelectedOptionsData { get; set; }
        public string Symbol { get; set; }
        public string CatalogNumber { get; set; }
        public string ExternalOfferId { get; set; }
        public decimal PriceBuy { get; set; }
    }

    public class SelectedOptionData
    {
        public string Name { get; set; }
        public string Prop { get; set; }
        public decimal Price { get; set; }
        public int OptionId { get; set; }
        public int VariantId { get; set; }
    }

    public class PaymentDetail
    {
        public int PaymentId { get; set; }
        public DateTime Date { get; set; }
        public string PaymentData { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
    }

    public class ShipmentDetail
    {
        public int OrderShipmentId { get; set; }
        public string Service { get; set; }
        public string TrackingNumber { get; set; }
    }

    public class AdditionalField
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}

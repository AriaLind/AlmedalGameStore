﻿using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Orders;

public class OrderDto : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid PaymentId { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Phonenumber { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Delivery { get; set; }
}
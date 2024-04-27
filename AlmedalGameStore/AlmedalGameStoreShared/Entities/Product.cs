﻿using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Entities;

public class Product : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int AgeRequirement { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public string? CoverPicturePath { get; set; }
    public int Stock { get; set; }
    public int UnitsSold { get; set; }
    public virtual List<string>? Categories { get; set; }
    public bool IsPhysical { get; set; }
    public virtual List<Review>? Reviews { get; set; }
}
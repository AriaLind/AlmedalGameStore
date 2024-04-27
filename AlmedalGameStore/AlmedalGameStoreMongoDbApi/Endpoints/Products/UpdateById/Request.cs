﻿using AlmedalGameStoreShared.Dtos.Reviews;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Products.UpdateById;

public class Request
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
    // TODO: Se över reviews GUID/ID
    public virtual List<ReviewDto>? Reviews { get; set; }
}
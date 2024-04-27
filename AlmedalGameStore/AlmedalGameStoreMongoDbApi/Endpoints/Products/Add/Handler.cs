    using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Dtos.Reviews;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Products.Add;
    
public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var product = new ProductDto()
        {
            Id = Guid.NewGuid(),
            AgeRequirement = req.AgeRequirement,
            Categories = req.Categories,
            CoverPicturePath = req.CoverPicturePath,
            Description = req.Description,
            Name = req.Name,
            Price = req.Price,
            IsPhysical = req.IsPhysical,
            ReleaseDate = req.ReleaseDate,
            Stock = req.Stock,
            UnitsSold = req.UnitsSold,
            Reviews = req.Reviews != null
                ? req.Reviews.Select(review => new ReviewDto
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null
        };

        if (product is null)
        {
            return TypedResults.BadRequest();
        }

        var allProducts = await mongoDbUnitOfWork.ProductRepository.GetAllAsync();

        if (allProducts.Any(p => p.Name == product.Name))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.ProductRepository.AddAsync(product);

        return TypedResults.Ok();
    }
}
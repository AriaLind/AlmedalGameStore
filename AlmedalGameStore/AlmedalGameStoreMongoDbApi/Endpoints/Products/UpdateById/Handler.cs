using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Dtos.Reviews;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Products.UpdateById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/products/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var products = await mongoDbUnitOfWork.ProductRepository.GetAllAsync();

        if (!products.Any(p => p.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var product = await mongoDbUnitOfWork.ProductRepository.GetByIdAsync(req.Id);

        product = new ProductDto()
        {
            Id = req.Id,
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

        await mongoDbUnitOfWork.ProductRepository.UpdateAsync(product);

        return TypedResults.Ok();
    }
}
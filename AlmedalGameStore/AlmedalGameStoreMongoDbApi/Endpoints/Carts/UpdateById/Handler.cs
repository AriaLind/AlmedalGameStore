using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Carts;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Dtos.Reviews;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Carts.UpdateById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/carts/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var carts = await mongoDbUnitOfWork.CartRepository.GetAllAsync();

        if (!carts.Any(p => p.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var cart = await mongoDbUnitOfWork.CartRepository.GetByIdAsync(req.Id);

        cart = new CartDto
        {
            Id = req.Id,
            UserId = req.UserId,
            ProductDtoList = req.ProductDtoList != null
                ? req.ProductDtoList.Select(product => new ProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    AgeRequirement = product.AgeRequirement,
                    ReleaseDate = product.ReleaseDate,
                    CoverPicturePath = product.CoverPicturePath,
                    Stock = product.Stock,
                    Id = product.Id,
                    Reviews = product.Reviews != null
                        ? product.Reviews.Select(review => new ReviewDto
                        {
                            Rating = review.Rating,
                            Id = review.Id,
                            Description = review.Description,
                            Title = review.Title
                        }).ToList()
                        : null,
                    Categories = product.Categories
                }).ToList()
                : null,
            CheckedOut = req.CheckedOut,
            Date = req.Date
        };

        await mongoDbUnitOfWork.CartRepository.UpdateAsync(cart);

        return TypedResults.Ok();
    }
}
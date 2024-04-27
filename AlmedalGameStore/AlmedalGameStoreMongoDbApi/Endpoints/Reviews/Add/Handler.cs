using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Reviews;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.Add;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/reviews");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var review = new ReviewDto()
        {
            Id = Guid.NewGuid(),
            Rating = req.Rating,
            Description = req.Description,
            Title = req.Title
        };

        if (review is null)
        {
            return TypedResults.BadRequest();
        }

        var allReviews = await mongoDbUnitOfWork.ReviewRepository.GetAllAsync();

        if (allReviews.Any(r => r.Id.Equals(review.Id)))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.ReviewRepository.AddAsync(review);

        return TypedResults.Ok();
    }
}
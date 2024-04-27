using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.Delete;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/reviews/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var review = await mongoDbUnitOfWork.ReviewRepository.GetByIdAsync(req.Id);

        if (review is null)
        {
            return TypedResults.BadRequest();
        }

        var allReviews = await mongoDbUnitOfWork.ReviewRepository.GetAllAsync();

        if (!allReviews.Any(r => r.Id.Equals(review.Id)))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.ReviewRepository.DeleteAsync(req.Id);

        return TypedResults.Ok();
    }
}
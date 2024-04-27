using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.UpdateById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/reviews/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var reviews = await mongoDbUnitOfWork.ReviewRepository.GetAllAsync();

        if (!reviews.Any(p => p.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var review = await mongoDbUnitOfWork.ReviewRepository.GetByIdAsync(req.Id);

        review.Description = req.Description;
        review.Rating = req.Rating;
        review.Title = req.Title;

        await mongoDbUnitOfWork.ReviewRepository.UpdateAsync(review);

        return TypedResults.Ok();
    }
}
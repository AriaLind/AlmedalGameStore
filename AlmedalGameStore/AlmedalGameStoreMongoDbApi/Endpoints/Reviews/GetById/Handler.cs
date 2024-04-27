using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.GetById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/reviews/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var reviews = await mongoDbUnitOfWork.ReviewRepository.GetAllAsync();

        if (!reviews.Any(r => r.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var review = reviews.FirstOrDefault(p => p.Id.Equals(req.Id));

        return TypedResults.Ok(new Response
        {
            Review = review
        });
    }
}
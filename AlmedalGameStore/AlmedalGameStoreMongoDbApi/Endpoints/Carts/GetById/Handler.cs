using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Carts.GetById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/carts/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
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

        var cart = carts.FirstOrDefault(p => p.Id.Equals(req.Id));

        return TypedResults.Ok(new Response
        {
            Cart = cart
        });
    }
}
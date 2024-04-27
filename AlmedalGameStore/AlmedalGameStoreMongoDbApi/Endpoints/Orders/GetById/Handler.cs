using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.GetById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/orders/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var orders = await mongoDbUnitOfWork.OrderRepository.GetAllAsync();

        if (!orders.Any(o => o.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var order = orders.FirstOrDefault(o => o.Id.Equals(req.Id));

        return TypedResults.Ok(new Response
        {
            Order = order
        });
    }
}
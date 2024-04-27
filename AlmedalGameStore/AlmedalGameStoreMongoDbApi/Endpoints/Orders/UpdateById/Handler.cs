using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.UpdateById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/orders/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
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

        var order = await mongoDbUnitOfWork.OrderRepository.GetByIdAsync(req.Id);

        // TODO: Sätta values på order

        await mongoDbUnitOfWork.OrderRepository.UpdateAsync(order);

        return TypedResults.Ok();
    }
}
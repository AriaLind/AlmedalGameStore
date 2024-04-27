using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.GetAll;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/orders");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var orders = await mongoDbUnitOfWork.OrderRepository.GetAllAsync();

        await SendAsync(new Response
        {
            Orders = orders
        });
    }
}
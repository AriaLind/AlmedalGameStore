using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.GetAll;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/payments");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var payments = await mongoDbUnitOfWork.PaymentRepository.GetAllAsync();

        await SendAsync(new Response
        {
            Payments = payments
        });
    }
}
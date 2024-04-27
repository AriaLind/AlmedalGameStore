using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Payments;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.Add;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/payments");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var payment = new PaymentDto()
        {
            Id = Guid.NewGuid(),
            Type = req.Type
        };

        if (payment is null)
        {
            return TypedResults.BadRequest();
        }

        var allPayments = await mongoDbUnitOfWork.PaymentRepository.GetAllAsync();

        if (allPayments.Any(p => p.Id.Equals(payment.Id)))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.PaymentRepository.AddAsync(payment);

        return TypedResults.Ok();
    }
}
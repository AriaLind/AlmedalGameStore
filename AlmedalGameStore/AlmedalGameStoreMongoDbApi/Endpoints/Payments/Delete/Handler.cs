using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.Delete;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/payments/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var payment = await mongoDbUnitOfWork.PaymentRepository.GetByIdAsync(req.Id);

        if (payment is null)
        {
            return TypedResults.BadRequest();
        }

        var allPayments = await mongoDbUnitOfWork.PaymentRepository.GetAllAsync();

        if (!allPayments.Any(p => p.Id.Equals(payment.Id)))
        {
            return TypedResults.BadRequest();
        }

        await mongoDbUnitOfWork.PaymentRepository.DeleteAsync(req.Id);

        return TypedResults.Ok();
    }
}
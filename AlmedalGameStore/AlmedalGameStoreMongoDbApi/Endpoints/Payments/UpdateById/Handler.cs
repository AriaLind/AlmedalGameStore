using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.UpdateById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/payments/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var payments = await mongoDbUnitOfWork.PaymentRepository.GetAllAsync();

        if (!payments.Any(p => p.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var payment = await mongoDbUnitOfWork.PaymentRepository.GetByIdAsync(req.Id);
        
        payment.Type = req.Type;

        await mongoDbUnitOfWork.PaymentRepository.UpdateAsync(payment);

        return TypedResults.Ok();
    }
}
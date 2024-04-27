using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.GetById;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/payments/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.Id == null || req.Id == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }

        var payments = await mongoDbUnitOfWork.PaymentRepository.GetAllAsync();

        if (!payments.Any(r => r.Id.Equals(req.Id)))
        {
            return TypedResults.NotFound();
        }

        var payment = payments.FirstOrDefault(p => p.Id.Equals(req.Id));

        return TypedResults.Ok(new Response
        {
            Payment = payment
        });
    }
}
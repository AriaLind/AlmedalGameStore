using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.Delete;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
	public override void Configure()
	{
		Delete("/events/{id}");
		AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
	{

		if (req.Id == null || req.Id == Guid.Empty)
		{
			return TypedResults.BadRequest();
		}

		var products = await mongoDbUnitOfWork.EventRepository.GetAllAsync();

		if (!products.Any(p => p.Id.Equals(req.Id)))
		{
			return TypedResults.NotFound();
		}

		await mongoDbUnitOfWork.EventRepository.DeleteAsync(req.Id);

		return TypedResults.Ok();
	}
}

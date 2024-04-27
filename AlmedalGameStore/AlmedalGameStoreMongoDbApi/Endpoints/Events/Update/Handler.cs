using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.Update;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
	public override void Configure()
	{
		Put("/events/{id}");
		AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
	{
		if (req.Id == null || req.Id == Guid.Empty)
		{
			return TypedResults.BadRequest();
		}

		var allEvents = await mongoDbUnitOfWork.EventRepository.GetAllAsync();

		if (!allEvents.Any(p => p.Id.Equals(req.Id)))
		{
			return TypedResults.NotFound();
		}

		var storeEvent = await mongoDbUnitOfWork.EventRepository.GetByIdAsync(req.Id);

		storeEvent.TicketId = req.TicketId;
		storeEvent.Name = req.Name;
		storeEvent.Description = req.Description;
		storeEvent.Price = req.Price;
		storeEvent.Stock = req.Stock;
		storeEvent.UnitsSold = req.UnitsSold;
		
		await mongoDbUnitOfWork.EventRepository.UpdateAsync(storeEvent);

		return TypedResults.Ok();
		
	}
}

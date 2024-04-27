using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.GetAll;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<EmptyRequest, Response>
{
	public override void Configure()
	{
		Get("/events");
		AllowAnonymous();
	}

	public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
	{
		var allEvents = await mongoDbUnitOfWork.EventRepository.GetAllAsync();

		await SendAsync(new Response
		{
			Events = allEvents
		});
	}
}

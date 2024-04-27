    using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Event;
using AlmedalGameStoreShared.Dtos.Reviews;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.Add;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
	public override void Configure()
	{
		Post("/events");
		AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
	{
		var storeEvent = new EventDto()
		{
			Id = Guid.NewGuid(),
			Categories = req.Categories,
			CoverPicturePath = req.CoverPicturePath,
			Date = req.Date,
			Description = req.Description,
			Name = req.Name,
			Stock = req.Stock,
            UnitsSold = req.UnitsSold,
            Price = req.Price,
			Reviews = req.Reviews != null
                ? req.Reviews.Select(review => new ReviewDto
                {
                    Rating = review.Rating,
                    Id = review.Id,
                    Description = review.Description,
                    Title = review.Title
                }).ToList()
                : null,
            TicketId = req.TicketId
			
		};
		
		if (storeEvent is null)
		{
			return TypedResults.BadRequest();
		}

		var allEvents = await mongoDbUnitOfWork.EventRepository.GetAllAsync();

		if (allEvents.Any(p => p.Name.Equals(storeEvent.Name)))
		{
			return TypedResults.BadRequest();
		}

		await mongoDbUnitOfWork.EventRepository.AddAsync(storeEvent);

		return TypedResults.Ok();

		
	}
}

	

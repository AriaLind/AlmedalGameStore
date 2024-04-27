using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.GetAll;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/reviews");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var reviews = await mongoDbUnitOfWork.ReviewRepository.GetAllAsync();

        await SendAsync(new Response
        {
            Reviews = reviews
        });
    }
}
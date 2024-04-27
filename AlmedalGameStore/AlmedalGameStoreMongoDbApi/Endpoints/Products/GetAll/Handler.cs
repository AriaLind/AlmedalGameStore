using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Products.GetAll;

public class Handler(MongoDbUnitOfWork mongoDbUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var products = await mongoDbUnitOfWork.ProductRepository.GetAllAsync();

        await SendAsync(new Response
        {
            Products = products
        });
    }

    
}
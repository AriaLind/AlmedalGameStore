using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.Add;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, EmptyResponse>
{
    public override void Configure()
    {
        Post("/roles");
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        await client.PostAsJsonAsync("/roles", req);
    }
}
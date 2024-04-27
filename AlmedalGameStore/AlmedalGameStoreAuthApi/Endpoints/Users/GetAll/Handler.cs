using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Users;
using AlmedalGameStoreShared.Entities;
using FastEndpoints;

namespace AlmedalGameStoreAuthApi.Endpoints.Users.GetAll;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/users");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync("/users");

        if (!response.IsSuccessStatusCode)
        {
            await SendAsync(new Response
            {
                Users = new List<User>()
            });
        }

        var users = await response.Content.ReadFromJsonAsync<UsersList>();

        await SendAsync(new Response
        {
            Users = users.Users
        });
    }
}
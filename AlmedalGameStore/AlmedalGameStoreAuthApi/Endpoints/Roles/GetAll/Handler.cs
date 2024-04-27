using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Roles;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.GetAll;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/roles");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync("roles");

        if (!response.IsSuccessStatusCode)
        {
            await SendAsync(new Response
            {
                Roles = Enumerable.Empty<IdentityRole>()
            });
        }

        var roles = await response.Content.ReadFromJsonAsync<IdentityRolesList>();

        await SendAsync(new Response
        {
            Roles = roles.Roles
        });
    }
}
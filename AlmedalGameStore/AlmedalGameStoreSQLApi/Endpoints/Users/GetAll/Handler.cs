using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.GetAll;

public class Handler(UserManager<User> userManager) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var users = await userManager.Users.ToListAsync();

        await SendAsync(new Response
        {
            Users = users
        });
    }
}
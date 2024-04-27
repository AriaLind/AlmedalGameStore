using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.GetAll;

public class Handler(RoleManager<IdentityRole> roleManager) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/roles");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var roles = roleManager.Roles.ToList();

        await SendAsync(new Response
        {
            Roles = roles
        });
    }
}
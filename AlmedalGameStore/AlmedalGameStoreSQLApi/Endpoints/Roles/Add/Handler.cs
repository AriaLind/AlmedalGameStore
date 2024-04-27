using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.Add;

public class Handler(RoleManager<IdentityRole> roleManager) : Endpoint<Request, EmptyResponse>
{
    public override void Configure()
    {
        Post("/roles");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var role = new IdentityRole(req.Name);

        await roleManager.CreateAsync(role);
    }
}
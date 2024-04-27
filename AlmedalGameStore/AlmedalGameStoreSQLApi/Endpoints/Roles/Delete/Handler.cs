using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.Delete;

public class Handler(RoleManager<IdentityRole> roleManager) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/roles/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var role = await roleManager.FindByIdAsync(req.Id);

        if (role is null)
        {
            return TypedResults.NotFound();
        }

        await roleManager.DeleteAsync(role);

        return TypedResults.Ok();
    }
}
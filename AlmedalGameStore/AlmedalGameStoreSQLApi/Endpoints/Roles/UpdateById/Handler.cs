using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.UpdateById;

public class Handler(RoleManager<IdentityRole> roleManager) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/roles/{id}/{name}");
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

        role.Name = req.Name;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
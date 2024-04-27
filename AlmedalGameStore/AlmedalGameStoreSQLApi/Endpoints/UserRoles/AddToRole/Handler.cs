using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.AddToRole;

public class Handler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/user-roles");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var role = await roleManager.FindByNameAsync(req.RoleName);

        if (role is null)
        {
            return TypedResults.BadRequest();
        }

        var user = await userManager.FindByIdAsync(req.Id);

        if (user is null)
        {
            return TypedResults.BadRequest();
        }

        var result = await userManager.AddToRoleAsync(user, role.Name);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
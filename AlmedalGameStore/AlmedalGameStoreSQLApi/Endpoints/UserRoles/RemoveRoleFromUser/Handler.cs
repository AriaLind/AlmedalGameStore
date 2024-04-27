using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.RemoveRoleFromUser;

public class Handler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Delete("/user-roles/{userId}/{roleName}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(req.UserId);

        if (user is null)
        {
            return TypedResults.BadRequest();
        }

        var role = await roleManager.FindByNameAsync(req.RoleName);

        if (role is null)
        {
            return TypedResults.BadRequest();
        }

        var result = await userManager.RemoveFromRoleAsync(user, role.Name);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
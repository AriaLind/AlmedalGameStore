using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.GetAllUsersWithRole;

public class Handler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    : Endpoint<Request, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Get("/user-roles/get-by-role/{roleName}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var users = await userManager.GetUsersInRoleAsync(req.RoleName);

        if (users is null)
        {
            return TypedResults.BadRequest();
        }

        var response = new Response()
        {
            Users = users
        };

        return TypedResults.Ok(response);
    }
}

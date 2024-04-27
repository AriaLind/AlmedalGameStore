using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreSQLApi.Endpoints.UserRoles.GetAllRoles;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.GetAllRolesFromUser;

public class Handler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    : Endpoint<Request, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Get("/user-roles/get-by-user/{Name}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await userManager.FindByNameAsync(req.Name);

        if (user is null)
        {
            return TypedResults.BadRequest();
        }

        var roles = await userManager.GetRolesAsync(user);

        var response = new Response
        {
            UserRoles = roles.ToList()
        };

        return TypedResults.Ok(response);
    }
}
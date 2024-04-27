using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.Delete;

public class Handler(UserManager<User> userManager) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/users/{email}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await userManager.FindByNameAsync(req.UserName);

        if (user is null)
        {
            return TypedResults.BadRequest(); 
        }

        await userManager.DeleteAsync(user);

        return TypedResults.Ok();
    }
}
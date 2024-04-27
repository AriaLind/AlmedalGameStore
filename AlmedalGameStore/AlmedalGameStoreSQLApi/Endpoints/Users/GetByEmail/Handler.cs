using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreSQLApi.Endpoints.Users.GetById;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.GetByEmail;

public class Handler(UserManager<User> userManager) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/users/{email}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new Response
        {
            User = user
        });
    }
}
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.GetById;

public class Handler(RoleManager<IdentityRole> roleManager)
    : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/roles/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req,
        CancellationToken ct)
    {
        if (req.Id.IsNullOrEmpty())
        {
            return TypedResults.BadRequest();
        }

        var role = await roleManager.FindByIdAsync(req.Id);

        if (role == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new Response
        {
            Role = role
        });
    }
}
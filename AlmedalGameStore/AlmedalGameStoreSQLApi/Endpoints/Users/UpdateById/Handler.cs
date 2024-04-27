using AlmedalGameStoreShared.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.UpdateById;

public class Handler(UserManager<User> userManager) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
        Options(b => b.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>());
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(req.Id);

        if (user == null)
        {
            return TypedResults.BadRequest();

        }

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        if (!req.NewPassword.IsNullOrEmpty() && req.CurrentPassword != req.NewPassword)
        {
            var result = await userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
        }

        if (!req.UserName.IsNullOrEmpty() && req.UserName != user.UserName)
        {
            var result = await userManager.SetUserNameAsync(user, req.UserName);
        }

        if (!req.Email.IsNullOrEmpty() && req.Email != user.Email)
        {
            var result = await userManager.SetEmailAsync(user, req.Email);
        }

        return TypedResults.Ok();
    }
}
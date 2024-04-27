using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.UserRoles.RemoveRoleFromUser;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Delete("/user-roles/{userId}/{roleName}");
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreAuthApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.DeleteAsync($"/user-roles/{req.UserId}/{req.RoleName}", ct);

        if (!response.IsSuccessStatusCode)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
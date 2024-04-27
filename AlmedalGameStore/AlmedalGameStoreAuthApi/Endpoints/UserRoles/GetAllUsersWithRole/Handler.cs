using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.UserRoles.GetAllUsersWithRole;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Get("/user-roles/get-by-role/{rolename}");
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync($"/user-roles/get-by-role/{req.RoleName}");

        if (!response.IsSuccessStatusCode)
        {
            return TypedResults.BadRequest();
        }

        var users = await response.Content.ReadFromJsonAsync<UsersList>();

        return TypedResults.Ok(new Response
        {
            Users = users.Users
        });
    }
}
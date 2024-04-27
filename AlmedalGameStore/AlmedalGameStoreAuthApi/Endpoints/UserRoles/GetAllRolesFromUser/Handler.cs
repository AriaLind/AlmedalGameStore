using System.Text.Json;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Roles;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.UserRoles.GetAllRolesFromUser;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Get("/user-roles/get-by-user/{Name}");
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync($"/user-roles/get-by-user/{req.Name}");

        if (!response.IsSuccessStatusCode)
        {
            return TypedResults.BadRequest();
        }

        var content = await response.Content.ReadAsStringAsync();

        var roles = JsonSerializer.Deserialize<RolesList>(content);

        return TypedResults.Ok(new Response
        {
            Roles = roles.userRoles
        });
    }
}
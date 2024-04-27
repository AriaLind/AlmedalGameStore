using System.Net;
using System.Text.Json;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Roles;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.GetById;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/roles/{id}");
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {

        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync($"/roles/{req.Id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return TypedResults.BadRequest();
        }

        var content = await response.Content.ReadAsStringAsync();
        
        //TODO: Fixa deserialiseringsfel
        var role = JsonSerializer.Deserialize<SingleIdentityRole>(content);

        return TypedResults.Ok(new Response
        {
            Role = role.role
        });
    }
}
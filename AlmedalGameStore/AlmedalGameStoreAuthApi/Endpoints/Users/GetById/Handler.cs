using System.Net;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.Users.GetById;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/users/{id}");
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.GetAsync($"/users/{req.Id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return TypedResults.BadRequest();
        }

        var user = await response.Content.ReadFromJsonAsync<SingleUser>(ct);

        return TypedResults.Ok(new Response
        {
            User = user.User
        });
    }
}
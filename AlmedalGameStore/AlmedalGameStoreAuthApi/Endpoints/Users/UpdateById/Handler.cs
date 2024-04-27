using System.Net;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.Users.UpdateById;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/users/{id}");
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.PutAsJsonAsync($"/users/{req.Id}", req);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return TypedResults.Ok();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.BadRequest();
    }
}
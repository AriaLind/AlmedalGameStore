using System.Net;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.Delete;

public class Handler(IHttpClientFactory factory, SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Delete("/roles/{id}");
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        client.DefaultRequestHeaders.Add("X-Api-Key", key.Key);

        var response = await client.DeleteAsync($"/roles/{req.Id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok();
    }
}
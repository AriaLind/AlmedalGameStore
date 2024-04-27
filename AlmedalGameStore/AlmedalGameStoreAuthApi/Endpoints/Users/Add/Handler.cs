using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreAuthApi.Endpoints.Users.Add;

public class Handler(IHttpClientFactory factory) : Endpoint<Request, Results<Ok, BadRequest>>
{

    public override void Configure()
    {
        Post("/users");
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var client = factory.CreateClient("AlmedalGameStoreSqlApi");

        var response = await client.PostAsJsonAsync("/users", req);

        if (response.IsSuccessStatusCode)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest();
    }
}
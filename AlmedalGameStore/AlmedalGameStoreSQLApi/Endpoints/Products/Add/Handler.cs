using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.Add;

public class Handler : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/users");
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        return TypedResults.BadRequest();

        return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}
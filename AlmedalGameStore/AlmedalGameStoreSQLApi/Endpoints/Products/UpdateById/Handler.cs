using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.UpdateById;

public class Handler : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("/users/{id}");
    }

    public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        return TypedResults.NotFound();

        return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}
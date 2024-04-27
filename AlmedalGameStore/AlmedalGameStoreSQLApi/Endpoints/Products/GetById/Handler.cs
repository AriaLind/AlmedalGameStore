using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.GetById;

public class Handler : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("/users/{id:int}");
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        return TypedResults.NotFound();

        return TypedResults.BadRequest();

        return TypedResults.Ok(new Response
        {

        });
    }
}
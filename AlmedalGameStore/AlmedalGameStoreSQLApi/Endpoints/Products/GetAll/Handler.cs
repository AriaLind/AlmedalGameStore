using FastEndpoints;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.GetAll;

public class Handler : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        SendAsync(new Response
        {
            
        });
    }
}
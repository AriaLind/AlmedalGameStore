using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints;

namespace AlmedalGameStoreAuthApi.Endpoints.AuthKeys.Validate;

public class Handler(SqlUnitOfWork sqlUnitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/validate-auth-key/{name}/{key}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken)
    {
        var test = await sqlUnitOfWork.AuthKeyRepository.GetByNameAsync(req.Name);

        if (test is null)
        {
            // Return false directly
            await SendAsync(new Response()
            {
                IsValid = false 

            });
        }

        // Construct the response
        var response = new Response()
        {
            IsValid = test.Key.Equals(req.Key)
        };

        // Send the response asynchronously
        await SendAsync(response);

    }
}
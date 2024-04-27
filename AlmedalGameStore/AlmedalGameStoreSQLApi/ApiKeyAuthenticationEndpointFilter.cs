using System.Text.Json;
using AlmedalGameStoreShared.Dtos.ApiKey;

namespace AlmedalGameStoreSQLApi;

public class ApiKeyAuthenticationEndpointFilter : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    private readonly HttpClient _httpClient;

    public ApiKeyAuthenticationEndpointFilter()
    {
        _httpClient = new HttpClient();

        // Sätter basadressen för HTTP-klienten till adressen för AuthApiet för att sedan validera nyckeln
        _httpClient.BaseAddress = new Uri("https://localhost:7007");
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (!await IsApiKeyValid(apiKey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }

    private async Task<bool> IsApiKeyValid(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        var response = await _httpClient.GetAsync($"/validate-auth-key/{"SqlApi"}/{apiKey}");

        if (response == null)
        {
            return false;
        }

        var correctKey = JsonSerializer.Deserialize<ValidateResponse>(await response.Content.ReadAsStringAsync());

        return correctKey.isValid;
    }
}
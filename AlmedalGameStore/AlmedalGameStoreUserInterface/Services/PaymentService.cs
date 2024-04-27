using AlmedalGameStoreShared.Dtos.Payments;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class PaymentService : IService<PaymentDto, Guid>
{
    private readonly HttpClient _httpClient;

    public PaymentService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }

    public async Task<PaymentDto> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"payments/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var payment = await response.Content.ReadFromJsonAsync<PaymentDto>();

        return payment;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("payments");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var payments = await response.Content.ReadFromJsonAsync<PaymentListDto>();

        return payments.Payments ?? Enumerable.Empty<PaymentDto>();
    }

    public async Task AddAsync(PaymentDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("payments", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task UpdateAsync(PaymentDto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"payments/{entity.Id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"payments/{id}");

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }
}
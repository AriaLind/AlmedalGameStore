using AlmedalGameStoreShared.Dtos.Orders;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class OrderService : IService<OrderDto, Guid>
{
    private readonly HttpClient _httpClient;

    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }

    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"orders/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new OrderDto();
        }

        var content = await response.Content.ReadFromJsonAsync<OrderDto>();

        return content;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("orders");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<OrderDto>();
        }

        var content = await response.Content.ReadFromJsonAsync<OrderListDto>();

        return content.Orders ?? Enumerable.Empty<OrderDto>();
    }

    public async Task AddAsync(OrderDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
        }
    }

    public async Task UpdateAsync(OrderDto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"orders/{entity.Id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"orders/{id}");

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
        }
    }
}
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Carts;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class CartService : IService<CartDto, Guid>
{
    private readonly HttpClient _httpClient;

    public CartService(IHttpClientFactory httpClientFactory, SqlUnitOfWork sqlUnitOfWork)
    {
        var key = sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("MongoDbApi");
        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }

    public async Task<CartDto> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"carts/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new CartDto();
        }

        var content = await response.Content.ReadFromJsonAsync<CartDto>();

        return content;
    }

    public async Task<IEnumerable<CartDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("carts");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<CartDto>();
        }

        // Deserialize the response content into CartListDto
        var content = await response.Content.ReadFromJsonAsync<CartListDto>();

        // Extract the carts from CartListDto
        var carts = content?.Carts;

        // If carts is null, return an empty enumerable; otherwise, return the carts
        return carts ?? Enumerable.Empty<CartDto>();
    }


    public async Task AddAsync(CartDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("carts", entity);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add cart");
        }
    }

    public async Task UpdateAsync(CartDto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"carts/{entity.Id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception("Failed to update cart");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"carts/{id}");

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception("Failed to delete cart");
        }
    }
}
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class ProductService : IService<ProductDto, Guid>
{
    private readonly HttpClient _httpClient;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        return product;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("products");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<ProductDto>();
        }

        var content = await response.Content.ReadFromJsonAsync<ProductListDto>();

        return content.Products ?? Enumerable.Empty<ProductDto>();
    }

    public async Task AddAsync(ProductDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("products", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task UpdateAsync(ProductDto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"products/{entity.Id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }
}
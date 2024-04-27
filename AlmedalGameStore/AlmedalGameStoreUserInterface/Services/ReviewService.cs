using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class ReviewService : IService<ReviewDto, Guid>
{
    private readonly HttpClient _httpClient;

    public ReviewService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }
    public async Task<ReviewDto> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"reviews/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var payment = await response.Content.ReadFromJsonAsync<ReviewDto>();

        return payment;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("reviews");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var reviews = await response.Content.ReadFromJsonAsync<IEnumerable<ReviewDto>>();

        return reviews;
    }

    public async Task AddAsync(ReviewDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("reviews", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task UpdateAsync(ReviewDto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"reviews/{entity.Id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"reviews/{id}");

        if (!response.IsSuccessStatusCode)
        {
            //TODO: Handle error
            throw new Exception();
        }
    }
}
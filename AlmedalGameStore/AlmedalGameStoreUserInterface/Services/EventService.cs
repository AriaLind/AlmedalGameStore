using AlmedalGameStoreShared.Dtos.Event;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreUserInterface.Services;

public class EventService : IService<EventDto, Guid>
{

	private readonly HttpClient _httpClient;

	public EventService(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
	}

	public async Task<EventDto> GetByIdAsync(Guid id)
	{
		var response = await _httpClient.GetAsync($"events/{id}");

		if (!response.IsSuccessStatusCode)
		{
			return new EventDto();
		}

		var content = await response.Content.ReadFromJsonAsync<EventDto>();

		return content;
	}

	public async Task<IEnumerable<EventDto>> GetAllAsync()
	{
		var response = await _httpClient.GetAsync("events");

		if (!response.IsSuccessStatusCode)
		{
			return Enumerable.Empty<EventDto>();
		}

		var content = await response.Content.ReadFromJsonAsync<EventListDto>();

		return content.Events ?? Enumerable.Empty<EventDto>();
	}

	public async Task AddAsync(EventDto entity)
	{
		var response = await _httpClient.PostAsJsonAsync("events", entity);

		if (!response.IsSuccessStatusCode)
		{
			return;
		}
	}

	public async Task UpdateAsync(EventDto entity)
	{
		var response = await _httpClient.PutAsJsonAsync($"events/{entity.Id}", entity);

		if (!response.IsSuccessStatusCode)
		{
			// TODO: Handle error
		}
	}

	public async Task DeleteAsync(Guid id)
	{
		var response = await _httpClient.DeleteAsync($"events/{id}");

		if (!response.IsSuccessStatusCode)
		{
			// TODO: Handle error
		}


	}
}
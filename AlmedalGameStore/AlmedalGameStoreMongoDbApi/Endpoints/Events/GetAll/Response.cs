using AlmedalGameStoreShared.Dtos.Event;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.GetAll;

public class Response
{
	public IEnumerable<EventDto> Events { get; set; }
}
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Event;

public class EventDto : ProductDto, IEntity<Guid>
{
	public string TicketId { get; set; }
	public DateTime Date { get; set; }
}
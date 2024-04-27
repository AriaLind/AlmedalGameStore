using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Entities;

public class Event : Product, IEntity<Guid>
{
	public DateTime Date { get; set; }
    public string TicketId { get; set; }

}
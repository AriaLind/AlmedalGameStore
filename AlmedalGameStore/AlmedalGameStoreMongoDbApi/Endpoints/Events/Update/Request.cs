namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.Update;

public class Request
{
	public Guid Id { get; set; }
	public string TicketId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }
	public int Stock { get; set; }
    public int UnitsSold { get; set; }

}
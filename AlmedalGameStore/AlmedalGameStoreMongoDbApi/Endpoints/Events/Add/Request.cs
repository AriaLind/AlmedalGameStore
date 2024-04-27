using AlmedalGameStoreShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Events.Add;

public class Request
{
	public string TicketId { get; set; }
	[Required, MaxLength(50)]
	public string Name { get; set; }
	[Required, MaxLength(50)]
	public string Description { get; set; }

	public decimal Price { get; set; }
	public List<Review> Reviews { get; set; }
	public int Age { get; set; }
	public DateTime Date { get; set; }
	public List<string> Categories { get; set; }
	public string CoverPicturePath { get; set; }
	public int Stock { get; set; }
    public int UnitsSold { get; set; }
}
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Entities;

public class Cart : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public List<Product>? ProductList { get; set; }
    public bool CheckedOut { get; set; }
    public DateTime Date { get; set; }
}
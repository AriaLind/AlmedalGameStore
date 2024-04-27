using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Carts;

public class CartDto : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public List<ProductDto>? ProductDtoList { get; set; }
    public bool CheckedOut { get; set; }
    public DateTime Date { get; set; }
}
using AlmedalGameStoreShared.Dtos.Products;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Carts.UpdateById;

public class Request
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public List<ProductDto> ProductDtoList { get; set; }
    public bool CheckedOut { get; set; }
    public DateTime Date { get; set; }
}
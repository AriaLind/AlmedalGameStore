using AlmedalGameStoreShared.Dtos.Products;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Products.GetAll;

public class Response
{
    public IEnumerable<ProductDto> Products { get; set; }
}
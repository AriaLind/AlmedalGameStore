using AlmedalGameStoreShared.Dtos.Products;

namespace AlmedalGameStoreAuthApi.Endpoints.Products.GetAll;

public class Response
{
    public IEnumerable<ProductDto> Products { get; set; }
}
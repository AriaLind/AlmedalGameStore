using AlmedalGameStoreShared.Dtos.Carts;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Carts.GetAll;

public class Response
{
    public IEnumerable<CartDto> Carts { get; set; }
}
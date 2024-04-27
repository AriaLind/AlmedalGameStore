using AlmedalGameStoreShared.Dtos.Orders;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.GetAll;

public class Response
{
    public IEnumerable<OrderDto> Orders { get; set; }

}
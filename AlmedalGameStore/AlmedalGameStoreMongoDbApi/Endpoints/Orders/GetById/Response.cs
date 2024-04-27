using AlmedalGameStoreShared.Dtos.Orders;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Orders.GetById;

public class Response
{
    public OrderDto Order { get; set; }
}
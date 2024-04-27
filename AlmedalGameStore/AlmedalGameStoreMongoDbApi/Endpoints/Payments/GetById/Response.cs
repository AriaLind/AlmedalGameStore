
using AlmedalGameStoreShared.Dtos.Payments;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.GetById;

public class Response
{
    public PaymentDto Payment { get; set; }
}
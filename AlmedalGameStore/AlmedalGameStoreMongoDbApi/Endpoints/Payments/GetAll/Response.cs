using AlmedalGameStoreShared.Dtos.Payments;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.GetAll;

public class Response
{
    public IEnumerable<PaymentDto> Payments { get; set; }
}
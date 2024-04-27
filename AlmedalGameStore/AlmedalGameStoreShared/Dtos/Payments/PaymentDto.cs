using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Payments;

public class PaymentDto : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Type { get; set; }
}
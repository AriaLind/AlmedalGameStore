using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Entities;

public class Payment : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Type { get; set; }
}
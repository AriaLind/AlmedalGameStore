using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Entities;

public class Review : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}
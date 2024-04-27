using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Reviews;

public class ReviewDto : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}
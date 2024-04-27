namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.Add;

public class Request
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}
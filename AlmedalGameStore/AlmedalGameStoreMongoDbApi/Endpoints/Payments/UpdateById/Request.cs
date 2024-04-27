namespace AlmedalGameStoreMongoDbApi.Endpoints.Payments.UpdateById;

public class Request
{
    public Guid Id { get; set; }
    public string Type { get; set; }
}
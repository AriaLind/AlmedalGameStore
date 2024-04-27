using AlmedalGameStoreShared.Dtos.Reviews;

namespace AlmedalGameStoreMongoDbApi.Endpoints.Reviews.GetAll;

public class Response
{
    public IEnumerable<ReviewDto> Reviews { get; set; }
}
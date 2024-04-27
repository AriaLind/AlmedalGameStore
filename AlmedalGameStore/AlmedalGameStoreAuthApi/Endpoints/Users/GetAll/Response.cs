using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreAuthApi.Endpoints.Users.GetAll;

public class Response
{
    public IEnumerable<User> Users { get; set; }
}
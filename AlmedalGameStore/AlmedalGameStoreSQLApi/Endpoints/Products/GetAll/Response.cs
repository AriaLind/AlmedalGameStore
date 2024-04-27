using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreSQLApi.Endpoints.Users.GetAll;

public class Response
{
    public IEnumerable<User> Users { get; set; }
}
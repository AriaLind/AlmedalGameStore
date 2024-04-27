using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.GetAllUsersWithRole;

public class Response
{
    public IEnumerable<User> Users { get; set; }
}
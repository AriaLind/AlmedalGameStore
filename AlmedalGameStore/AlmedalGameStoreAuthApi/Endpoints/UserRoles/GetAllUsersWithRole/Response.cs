using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreAuthApi.Endpoints.UserRoles.GetAllUsersWithRole;

public class Response
{
    public IEnumerable<User> Users { get; set; }
}
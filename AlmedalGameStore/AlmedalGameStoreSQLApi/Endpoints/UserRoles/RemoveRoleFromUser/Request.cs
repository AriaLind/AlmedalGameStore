namespace AlmedalGameStoreSQLApi.Endpoints.UserRoles.RemoveRoleFromUser;

public class Request
{
    public string UserId { get; set; }
    public string RoleName { get; set; }
}
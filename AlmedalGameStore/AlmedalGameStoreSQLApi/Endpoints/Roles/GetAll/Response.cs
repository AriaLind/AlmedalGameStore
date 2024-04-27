using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.GetAll;

public class Response
{
    public IEnumerable<IdentityRole> Roles { get; set; }
}
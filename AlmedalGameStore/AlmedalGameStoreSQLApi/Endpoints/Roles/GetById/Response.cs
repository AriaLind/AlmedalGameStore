using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreSQLApi.Endpoints.Roles.GetById;

public class Response
{
    public IdentityRole Role { get; set; }
}
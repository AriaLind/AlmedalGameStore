using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.GetAll;

public class Response
{
    public IEnumerable<IdentityRole> Roles { get; set; }
}
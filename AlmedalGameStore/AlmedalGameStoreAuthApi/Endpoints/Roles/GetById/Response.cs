using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreAuthApi.Endpoints.Roles.GetById;

public class Response
{
    public IdentityRole Role { get; set; }
}
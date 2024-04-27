using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreShared.Dtos.Roles;

public class IdentityRolesList
{
    public IEnumerable<IdentityRole> Roles { get; set; }
}
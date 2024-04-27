using System.Collections;
using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreShared.Dtos.Users;

public class UsersList
{
    public IEnumerable<User> Users { get; set; }
    
}
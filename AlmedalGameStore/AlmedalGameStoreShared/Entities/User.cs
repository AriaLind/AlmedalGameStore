using AlmedalGameStoreShared.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AlmedalGameStoreShared.Entities;

public class User : IdentityUser, IEntity<string>
{
    //[Required, MaxLength(50)]
    //public string Address { get; set; }

    //[Required, MaxLength(6)] 
    //public string ZipCode { get; set; }

    //[Required, MaxLength(50)]
    //public string City { get; set; }

    //[Url]
    //public string ProfilePicturePath { get; set; }

    //[Required, MaxLength(50)]
    //public virtual List<User> FriendsList { get; set; }

    //[Url] 
    //public string DiscordLink { get; set; }

    //[Url] 
    //public string SteamLink { get; set; }
}
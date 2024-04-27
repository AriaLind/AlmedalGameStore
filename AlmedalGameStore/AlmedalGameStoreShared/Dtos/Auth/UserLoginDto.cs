using System.ComponentModel.DataAnnotations;

namespace AlmedalGameStoreShared.Dtos.Auth;

public class UserLoginDto
{

    [Required(ErrorMessage = "Användarnamn är obligatoriskt")] //Det står att det är email men det är egentligen användarnamn man använder för att logga in.
    public string Email { get; set; }

    [Required(ErrorMessage = "Lösenord är obligatoriskt"), MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken långt")]
    public string Password { get; set; }
}
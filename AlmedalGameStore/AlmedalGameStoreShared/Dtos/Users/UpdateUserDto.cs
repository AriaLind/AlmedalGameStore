using System.ComponentModel.DataAnnotations;

namespace AlmedalGameStoreShared.Dtos.Users;

public class UpdateUserDto
{
    public string Id { get; set; }

    [StringLength(50, ErrorMessage = "Användarnamnet måste vara minst 3 tecken långt.", MinimumLength = 3)]
    public string UserName { get; set; }

    [StringLength(100, ErrorMessage = "Lösenordet måste vara minst {2} tecken långt.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
    [Display(Name = "Bekräfta lösenord")]
    public string ConfirmNewPassword { get; set; }

    [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Nuvarande lösenord krävs för att ändra din information.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
}
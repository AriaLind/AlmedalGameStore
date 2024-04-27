using System.ComponentModel.DataAnnotations;

namespace AlmedalGameStoreShared.Dtos.Auth;

public class UserRegisterDto
{
    [Required(ErrorMessage = "E-post är obligatoriskt"), EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lösenord är obligatoriskt"), MinLength(8, ErrorMessage = "Lösenordet måste vara minst 8 tecken långt")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Bekräfta lösenord är obligatoriskt"), Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Du måste godkänna användarvillkoren"), Range(typeof(bool), "true", "true", ErrorMessage = "Du måste godkänna användarvillkoren")]
    public bool AgreedToTerms { get; set; }

    //[Required(ErrorMessage = "Förnamn är obligatoriskt"), MaxLength(50, ErrorMessage = "Förnamnet får inte vara längre än 50 tecken")]
    //public string FirstName { get; set; }

    //[Required(ErrorMessage = "Efternamn är obligatoriskt"), MaxLength(50, ErrorMessage = "Efternamnet får inte vara längre än 50 tecken")]
    //public string LastName { get; set; }

    //[Required(ErrorMessage = "Telefonnummer är obligatoriskt"), Phone(ErrorMessage = "Ogiltigt telefonnummerformat")]
    //public string PhoneNumber { get; set; }

    //[Required(ErrorMessage = "Adress är obligatoriskt"), MaxLength(100, ErrorMessage = "Adressen får inte vara längre än 100 tecken")]
    //public string Address { get; set; }
}
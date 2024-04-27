using System.ComponentModel.DataAnnotations;
using AlmedalGameStoreShared.Entities;

namespace AlmedalGameStoreShared.Models;

public class CheckoutModel
{
    [Required(ErrorMessage = "Förnamn är obligatoriskt")]
    [MinLength(2, ErrorMessage = "Förnamn måste vara minst 2 tecken långt")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Efternamn är obligatoriskt")]
    [MinLength(2, ErrorMessage = "Efternamn måste vara minst 2 tecken långt")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "E-postadress är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Adress är obligatoriskt")]
    [MinLength(2, ErrorMessage = "Adress måste vara minst 2 tecken långt")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Stad är obligatoriskt")]
    [MinLength(2, ErrorMessage = "Stad måste vara minst 2 tecken långt")]
    public string City { get; set; }

    [Required(ErrorMessage = "Postnummer är obligatoriskt")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Ogiltigt postnummer")]
    public string ZipCode { get; set; }

    [Required(ErrorMessage = "Telefonnummer är obligatoriskt")]
    [Phone(ErrorMessage = "Ogiltigt telefonnummer")]
    public string PhoneNumber { get; set; }

    public bool GetNews { get; set; }

    public Payment Payment { get; set; }

    [Required(ErrorMessage = "Namn på kort är obligatoriskt")]
    [MinLength(2, ErrorMessage = "Namn på kort måste vara minst 2 tecken långt")]
    public string NameOnCard { get; set; }

    [Required(ErrorMessage = "Kortnummer är obligatoriskt")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "Ogiltigt kortnummer")]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "Utgångsdatum är obligatoriskt")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Ogiltigt utgångsdatum")]
    public string ExpiryDate { get; set; }

    [Required(ErrorMessage = "CVV är obligatoriskt")]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Ogiltig CVV")]
    public string CVV { get; set; }
}

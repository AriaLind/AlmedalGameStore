using System.ComponentModel.DataAnnotations;
using AlmedalGameStoreShared.Dtos.Reviews;
using AlmedalGameStoreShared.Interfaces;

namespace AlmedalGameStoreShared.Dtos.Products;

public class ProductDto : IEntity<Guid>
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Namn är obligatoriskt")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Namn måste vara mellan 3 och 100 tecken")]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Pris är obligatoriskt")]
    [Range(0, double.MaxValue, ErrorMessage = "Priset måste vara ett icke-negativt värde")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Ålderskrav är obligatoriskt")]
    [Range(0, int.MaxValue, ErrorMessage = "Ålderskravet måste vara ett icke-negativt värde")]
    public int AgeRequirement { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? CoverPicturePath { get; set; }

    [Required(ErrorMessage = "Lager är obligatoriskt")]
    [Range(0, int.MaxValue, ErrorMessage = "Lagret måste vara ett icke-negativt värde")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Antal sålda är obligatoriskt")]
    [Range(0, int.MaxValue, ErrorMessage = "Antal sålda måste vara ett icke-negativt värde")]
    public int UnitsSold { get; set; }

    public virtual List<string>? Categories { get; set; } = new();

    [Required(ErrorMessage = "Fysisk status är obligatoriskt")]
    public bool IsPhysical { get; set; }

    public virtual List<ReviewDto>? Reviews { get; set; }
}
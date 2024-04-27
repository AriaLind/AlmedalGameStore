using AlmedalGameStoreUserInterface.Services;
using Microsoft.AspNetCore.Components;
using AlmedalGameStoreShared.Dtos.Products;
using Microsoft.AspNetCore.Components.Authorization;
namespace AlmedalGameStoreUserInterface.Components.Pages;

public partial class Home
{
    private string selectedSlide = "2";

    [Inject] private ProductService ProductService { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    public IEnumerable<ProductDto> top3SoldGames { get; set; } = new List<ProductDto>();

    protected override async Task OnInitializedAsync()
    {
        top3SoldGames = await GetTop3SoldGamesAsync();
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetTop3SoldGamesAsync()
    {
        var products = await ProductService.GetAllAsync();
        var top3SoldGames = products.OrderByDescending(p => p.UnitsSold).Take(3);
        return top3SoldGames;
    }

}

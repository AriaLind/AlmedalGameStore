using AlmedalGameStoreUserInterface.Services;
using Microsoft.AspNetCore.Components;

namespace AlmedalGameStoreUserInterface.Components.Pages.Shop;

public partial class Cart
{
    [Inject] private ActiveShoppingCartService activeShoppingCartService { get; set; }
    
	protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // This ensures that the JavaScript interop call is only made once,
            // during the first render of the component
            await activeShoppingCartService.Initialize();
            StateHasChanged(); // Ensure component re-renders after async call
        }
    }

}
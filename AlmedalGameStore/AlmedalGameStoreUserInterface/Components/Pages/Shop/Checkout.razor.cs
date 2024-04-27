using System.Text.Json;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Models;
using AlmedalGameStoreShared.Models.KlarnaModels;
using AlmedalGameStoreUserInterface.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AlmedalGameStoreUserInterface.Components.Pages.Shop;

public partial class Checkout
{
    [Inject] public ActiveShoppingCartService activeShoppingCartService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    public CheckoutModel CheckoutModel { get; set; }
    public UserData _UserData { get; set; }
    public OrderConfirmation orderConfirmation { get; set; } = new();

    public Checkout()
    {
        CheckoutModel = new CheckoutModel();
        CheckoutModel.Payment = new Payment();
    }

    public async Task InvalidSubmit()
    {
        Console.WriteLine("test");
    }

    public async Task HandleValidSubmit()
    {
        if (activeShoppingCartService.activeCart.ProductDtoList.Count == 0)
        {
            return;
        }

        _UserData = new UserData
        {
            first_name = CheckoutModel.FirstName,
            last_name = CheckoutModel.LastName,
            email = CheckoutModel.Email,
            address = new Address()
            {
                postal_code = CheckoutModel.ZipCode,
                city = CheckoutModel.City,
                street_address = CheckoutModel.Address,
                street_address2 = string.Empty,
                country = string.Empty,
                region = string.Empty,
            },
            phone = CheckoutModel.City,
            date_of_birth = CheckoutModel.ZipCode
        };
        
        //Order Confirmation Mail Start
        orderConfirmation.Name = _UserData.first_name;
        orderConfirmation.To = _UserData.email;
        orderConfirmation.Subject = "Orderbekräftelse";
        orderConfirmation.Date = DateTime.Now.ToString();
        orderConfirmation.Ordernumber = activeShoppingCartService.activeCart.Id.ToString();
        string products = "";
        decimal total = 0;
        foreach(var p in activeShoppingCartService.activeCart.ProductDtoList)
        {
            string product = $"{p.Name} {p.Price}kr <br>";
            total += p.Price;
            products += product;
        }
        products += $"<br> Total: {total}kr";
        orderConfirmation.EmailBody = products;

        string jsonOrderConfirmation = JsonSerializer.Serialize(orderConfirmation);
        using var client = new HttpClient();

        string logicAppUrl = Environment.GetEnvironmentVariable("LogicAppURL");
        var content = new StringContent(jsonOrderConfirmation, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync(logicAppUrl, content);
        response.EnsureSuccessStatusCode();
        //Order Confirmation Mail End

        await activeShoppingCartService.CheckOut(_UserData);

        _UserData = new UserData();
        _UserData.address = new Address();

        StateHasChanged();

        NavigationManager.NavigateTo("/shop/checkout/success");
    }

    private static Func<UserData, Task>? UserAuthenticatedAction;

    // Change the return type to 'async void'
    private async Task HandleUserAuthenticated(UserData userData)
    {
        _UserData = userData;

        // Await the asynchronous operation
        await activeShoppingCartService.CheckOut(_UserData);
        StateHasChanged();

        NavigationManager.NavigateTo("/shop/checkout/success");
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserAuthenticatedAction = HandleUserAuthenticated;
    }

    public async Task Dispose()
    {
        UserAuthenticatedAction = null;
    }

    [JSInvokable]
    public static void OnUserAuthenticated(JsonElement callbackData)
    {
        var userData = JsonSerializer.Deserialize<UserData>(callbackData.ToString());

        if (UserAuthenticatedAction != null)
        {
            UserAuthenticatedAction(userData);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // This ensures that the JavaScript interop call is only made once,
            // during the first render of the component
            await activeShoppingCartService.Initialize();
            foreach (var productDto in activeShoppingCartService.activeCart.ProductDtoList)
            {
                // Add the price of each product to the total cost
                TotalCost += productDto.Price;
            }
            StateHasChanged(); // Ensure component re-renders after async call
        }
    }

    private PromoCodeModel _promoCodeModel = new PromoCodeModel();

    private decimal TotalCost { get; set; }
    private bool ValidPromoCodeUsed { get; set; }

    private void CheckPromoCode(string promoCode)
    {
        if (string.IsNullOrEmpty(promoCode))
        {
            return;
        }

        if (promoCode == "EXAMPLECODE" && TotalCost > 50)
        {
            TotalCost = 0;

            foreach (var productDto in activeShoppingCartService.activeCart.ProductDtoList)
            {
                // Add the price of each product to the total cost
                TotalCost += productDto.Price;
            }

            TotalCost -= 50;
            ValidPromoCodeUsed = true;
        }
    }

    private RenderFragment PromoCodesFragment() => builder =>
    {
        builder.OpenElement(0, "li");
        builder.AddAttribute(1, "class", "list-group-item d-flex justify-content-between bg-light");
        builder.OpenElement(2, "div");
        builder.AddAttribute(3, "class", "text-success");
        builder.OpenElement(4, "h6");
        builder.AddAttribute(5, "class", "my-0");
        builder.AddContent(6, "Promo code");
        builder.CloseElement(); // Close h6
        builder.OpenElement(7, "small");
        builder.AddContent(8, "EXAMPLECODE");
        builder.CloseElement(); // Close small
        builder.CloseElement(); // Close div
        builder.OpenElement(9, "span");
        builder.AddAttribute(10, "class", "text-success");
        builder.AddContent(11, "-50 SEK");
        builder.CloseElement(); // Close span
        builder.CloseElement(); // Close li
    };

}
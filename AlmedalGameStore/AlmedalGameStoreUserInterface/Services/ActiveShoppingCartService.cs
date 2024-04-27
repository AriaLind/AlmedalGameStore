using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Carts;
using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreShared.Entities;
using Microsoft.JSInterop;
using AlmedalGameStoreShared.Dtos.Orders;
using JsonSerializer = System.Text.Json.JsonSerializer;
using AlmedalGameStoreShared.Models.KlarnaModels;

namespace AlmedalGameStoreUserInterface.Services;

public class ActiveShoppingCartService
{
    private readonly IJSRuntime _jsRuntime;

    private readonly HttpClient _httpClient;

    private readonly SqlUnitOfWork _sqlUnitOfWork;

    public CartDto activeCart { get; set; } = new();

    public List<ProductDto> UniqueCartProductDtos { get; set; } = new();


    public ActiveShoppingCartService(IJSRuntime jsRuntime, IHttpClientFactory httpClientFactory, SqlUnitOfWork sqlUnitOfWork)
    {
        _jsRuntime = jsRuntime;
        activeCart.ProductDtoList = new();
        _sqlUnitOfWork = sqlUnitOfWork;

        _httpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
    }

    public async Task Initialize()
    {
        var cartJsonRetrieved = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "cart");

        if (cartJsonRetrieved == null)
        {
            activeCart = new CartDto();
            activeCart.ProductDtoList = new();

            string cartJson = JsonSerializer.Serialize(activeCart);

            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "cart", cartJson);
            cartJsonRetrieved = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "cart");
        }

        activeCart = JsonSerializer.Deserialize<CartDto>(cartJsonRetrieved);

        UniqueCartProductDtos = (await GetUniqueCartProductDtos()).ToList();
    }

    public async Task AddItemToCart(ProductDto item)
    {
        activeCart.ProductDtoList.Add(item);

        var key = await _sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("MongoDbApi");

        if (!_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
        }

        // Fetch all carts from the backend API
        var allCartsResponse = await _httpClient.GetAsync("carts");
        allCartsResponse.EnsureSuccessStatusCode(); // Ensure successful response
                                                    // Deserialize the response into CartListDto
        var allCarts = await allCartsResponse.Content.ReadFromJsonAsync<CartListDto>();

        // Check if there's an existing cart with the same Id
        var existingCart = allCarts.Carts.FirstOrDefault(cart => cart.Id == activeCart.Id);

        if (existingCart != null)
        {
            // If an existing cart is found, update it with the new item
            existingCart.ProductDtoList.Add(item);

            // Update the existing cart in the backend API
            var updateResponse = await _httpClient.PutAsJsonAsync($"carts/{existingCart.Id}", existingCart);
            updateResponse.EnsureSuccessStatusCode(); // Ensure successful update
        }
        else
        {
            // If no existing cart is found, create a new cart in the backend API
            var createResponse = await _httpClient.PostAsJsonAsync("carts", activeCart);
            createResponse.EnsureSuccessStatusCode(); // Ensure successful creation
        }

        UniqueCartProductDtos = (await GetUniqueCartProductDtos()).ToList();


        // Save the updated cart to session storage
        await SaveCartToSessionStorage();
    }


    public async Task RemoveItemFromCart(Guid itemId)
    {
        var itemToRemove = activeCart.ProductDtoList.FirstOrDefault(item => item.Id == itemId);

        if (itemToRemove != null)
            activeCart.ProductDtoList.Remove(itemToRemove);

        if (CountQuantity(itemToRemove) <= 0)
        {
            UniqueCartProductDtos = (await GetUniqueCartProductDtos()).ToList();
        }

        await SaveCartToSessionStorage();
    }

    public async Task LitterBoxBtn(Guid itemId)
    {
        // Find all items with the specified ID in the cart
        var itemsToRemove = activeCart.ProductDtoList.Where(item => item.Id == itemId).ToList();

        // Loop through each item and remove it from the cart
        foreach (var itemToRemove in itemsToRemove)
        {
            activeCart.ProductDtoList.Remove(itemToRemove);
        }

        // If there are no more items with the specified ID in the cart, update the list of unique products
        if (!activeCart.ProductDtoList.Any(item => item.Id == itemId))
        {
            UniqueCartProductDtos = (await GetUniqueCartProductDtos()).ToList();
        }

        // Save the updated cart to session storage
        await SaveCartToSessionStorage();
    }

    public async Task CheckOut(UserData userData)
    {
        activeCart.CheckedOut = true;
        activeCart = new CartDto();
        activeCart.ProductDtoList = new();
        UniqueCartProductDtos = new();

        var key = await _sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("MongoDbApi");

        if (!_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
	        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
        }

		

        Payment payment = new Payment()
        {
            Id = Guid.NewGuid(),
            Type = "Klarna"
        };

        OrderDto order = new()
        {
            Id = Guid.NewGuid(),
            PurchaseDate = DateOnly.FromDateTime(DateTime.Now),
            PaymentId = payment.Id,
            CartId = activeCart.Id,
            ZipCode = userData.address.postal_code,
            Address = userData.address.street_address,
            City = userData.address.city,
            Delivery = "Email",
            Email = userData.email,
            Phonenumber = userData.phone
        };



        var createOrderResponse = await _httpClient.PostAsJsonAsync("orders", order);
        createOrderResponse.EnsureSuccessStatusCode();

        var createPaymentResponse = await _httpClient.PostAsJsonAsync("payments", payment);
        createPaymentResponse.EnsureSuccessStatusCode();

        //_httpClient.BaseAddress = new Uri("https://prod-82.westeurope.logic.azure.com/");

        //var sendEmailResponse = await _httpClient.PostAsJsonAsync("workflows/b65273d18afa46fbbccc7cdcf409ba92/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=N-9Rl8K2gXaIvkadFWz-uo0xqJ-4jjwfbBC9ehmH8S4", order);
        //sendEmailResponse.EnsureSuccessStatusCode();

        await SaveCartToSessionStorage();
    }

    public async Task SaveCartToSessionStorage()
    {
        if (_jsRuntime is IJSInProcessRuntime)
        {
            return;
        }

        // Serialize the activeCart object to a JSON string
        string cartJson = JsonSerializer.Serialize(activeCart);

        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "cart", cartJson);
    }

    public async Task<IEnumerable<ProductDto>> GetUniqueCartProductDtos()
    {
        // Ensure that activeCart is not null
        if (activeCart == null)
        {
            return Enumerable.Empty<ProductDto>(); // Return an empty collection
        }

        // Ensure that activeCart.ProductDtoList is not null
        if (activeCart.ProductDtoList == null)
        {
            return Enumerable.Empty<ProductDto>(); // Return an empty collection
        }

        // Perform the grouping operation
        var uniqueProducts = activeCart.ProductDtoList
            .GroupBy(p => p.Id) // Group by product ID
            .Select(g => g.First()); // Select the first item from each group

        return uniqueProducts;
    }


    public int CountQuantity(ProductDto prodDto)
    {
        if (prodDto is null)
        {
            return 0;
        }

        int totalQuantity = activeCart.ProductDtoList.Count(p => p.Id == prodDto.Id);

        return totalQuantity;
    }



}
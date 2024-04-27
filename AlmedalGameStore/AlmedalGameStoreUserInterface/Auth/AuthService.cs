using System.Security.Claims;
using System.Text.Json;
using AlmedalGameStoreShared.Dtos.Auth;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Carts;
using AlmedalGameStoreShared.Dtos.Roles;
using AlmedalGameStoreShared.Dtos.Users;
using AlmedalGameStoreShared.Entities;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using AlmedalGameStoreUserInterface.Services;

namespace AlmedalGameStoreUserInterface.Auth;

public class AuthService
{
    private readonly UserService _userService;
    private readonly HttpClient _authApiHttpClient;
    private readonly HttpClient _sqlApiHttpClient;
    private readonly HttpClient _mongoDbHttpClient;
    private readonly SqlUnitOfWork _sqlUnitOfWork;
    private readonly NavigationManager _navigationManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IJSRuntime _jsRuntime;
    private ActiveShoppingCartService _activeShoppingCartService;

    public AuthService(
        IHttpClientFactory httpClientFactory, 
        UserService userService, 
        SqlUnitOfWork sqlUnitOfWork, 
        NavigationManager navigationManager,
        AuthenticationStateProvider authenticationStateProvider,
        IJSRuntime iJsRuntime,
        ActiveShoppingCartService activeShoppingCartService)
    {
        _userService = userService;
        _authApiHttpClient = httpClientFactory.CreateClient("AlmedalGameStoreAuthApi");
        _sqlApiHttpClient = httpClientFactory.CreateClient("AlmedalGameStoreSqlApi");
        _mongoDbHttpClient = httpClientFactory.CreateClient("AlmedalGameStoreMongoDbApi");
        _sqlUnitOfWork = sqlUnitOfWork;
        _navigationManager = navigationManager;
        _authenticationStateProvider = authenticationStateProvider;
        _jsRuntime = iJsRuntime;
        _activeShoppingCartService = activeShoppingCartService;
    }

    public async Task<bool> RegisterAsync(UserRegisterDto registerModel)
    {
        PostToRegisterDto postToRegisterDto = new PostToRegisterDto
        {
            Email = registerModel.Email,
            Password = registerModel.Password
        };

        var result = await _authApiHttpClient.PostAsJsonAsync("/register", postToRegisterDto);

        await _authenticationStateProvider.GetAuthenticationStateAsync();

        _navigationManager.NavigateTo("/Account/Login");

        return result.IsSuccessStatusCode;
    }

    public async Task<bool> LoginAsync(UserLoginDto loginModel)
    {
        var result = await _authApiHttpClient.PostAsJsonAsync("/login", loginModel);

        if (result.IsSuccessStatusCode)
        {
            var response = await result.Content.ReadFromJsonAsync<LoginResponseDto>();

            var name = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Email),
                new Claim("token", response.AccessToken),
                new Claim("refreshToken", response.RefreshToken)
            }, "bearer");

            var key = await _sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

            if (!_sqlApiHttpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
            {
                _sqlApiHttpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
            }

            var userRoles = await _sqlApiHttpClient.GetAsync($"/user-roles/get-by-user/{loginModel.Email}");
            var rolesContent = await userRoles.Content.ReadAsStringAsync();
            try
            {
                var roles = JsonSerializer.Deserialize<RolesList>(rolesContent);
                foreach (var role in roles.userRoles)
                {
                    name.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            catch
            {

            }

            _userService.SetUser(new ClaimsPrincipal(name));

            var mongoDbKey = await _sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("MongoDbApi");

            if (!_mongoDbHttpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
            {
                _mongoDbHttpClient.DefaultRequestHeaders.Add("X-Api-Key", mongoDbKey.Key);
            }

            var getAllCartsResult = await _mongoDbHttpClient.GetAsync("carts");
            var allCarts = await getAllCartsResult.Content.ReadFromJsonAsync<CartListDto>();

            if (allCarts != null)
            {
                var user = await _sqlApiHttpClient.GetAsync($"/users/{loginModel.Email}");
                var userContent = await user.Content.ReadFromJsonAsync<SingleUser>();

                var cart = allCarts.Carts
                    .FirstOrDefault(c => c.UserId == userContent.User.Id && !c.CheckedOut);


                if (cart != null)
                {
                    _activeShoppingCartService.activeCart = cart;
                    _activeShoppingCartService.UniqueCartProductDtos = (await _activeShoppingCartService.GetUniqueCartProductDtos()).ToList();
                    await _activeShoppingCartService.SaveCartToSessionStorage();
                }
            }

            _navigationManager.NavigateTo("/");

            await _authenticationStateProvider.GetAuthenticationStateAsync();

            return true;

        }
        return false;
    }

    public async Task LogoutAsync()
    {
        _userService.SetUser(new ClaimsPrincipal());
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        _navigationManager.NavigateTo("/");

        await ((IJSInProcessRuntime)_jsRuntime).InvokeVoidAsync("location.reload");
    }
}
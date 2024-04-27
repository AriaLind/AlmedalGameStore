using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Users;
using AlmedalGameStoreShared.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using AlmedalGameStoreShared.Dtos.Auth;
using AlmedalGameStoreUserInterface.Auth;
using Blazorise;
using Blazorise.LoadingIndicator;
using AlmedalGameStoreShared.Dtos.Products;

namespace AlmedalGameStoreUserInterface.Components.Pages.Account;

public partial class EditProfile
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] IHttpClientFactory HttpClientFactory { get; set; }

    [Inject] SqlUnitOfWork SqlUnitOfWork { get; set; }

    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] AuthService AuthService { get; set; }

    [CascadingParameter]
    LoadingIndicator loadingIndicator { get; set; }

    public UpdateUserDto UpdateUserDto = new UpdateUserDto();

    public string UserId { get; set; }

    private HttpClient _httpClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetUser();
    }

    public async Task GetUser()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (state != null)
        {
            var key = await SqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

            _httpClient = HttpClientFactory.CreateClient("AlmedalGameStoreSqlApi");

            if (!_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
            }

            var getUserResponse = await _httpClient.GetAsync($"/users/{state.User.Identity.Name}");
            if (getUserResponse != null)
            {
                var user = await getUserResponse.Content.ReadFromJsonAsync<SingleUser>();

                UpdateUserDto.Email = user.User.Email;
                UpdateUserDto.UserName = user.User.UserName;
                UserId = user.User.Id;

                User = user.User;
            }

            StateHasChanged();
        }
    }

    public async Task UpdateUser()
    {
        await loadingIndicator.Show();
        var key = await SqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        _httpClient = HttpClientFactory.CreateClient("AlmedalGameStoreSqlApi");

        if (!_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
        }

        var response = await _httpClient.PutAsJsonAsync($"/users/{UserId}", UpdateUserDto);

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/");
        }

        StateHasChanged();

        await loadingIndicator.Hide();
    }

    bool confirmDeleteUserModal = false;

    public User User { get; set; }

    Task OpenDeleteUserModal()
    {
        confirmDeleteUserModal = true;

        return Task.CompletedTask;
    }

    Task CloseDeleteProductModal()
    {
        confirmDeleteUserModal = false;

        return Task.CompletedTask;
    }

    public async Task DeleteUser()
    {
        await loadingIndicator.Show();
        var key = await SqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        _httpClient = HttpClientFactory.CreateClient("AlmedalGameStoreSqlApi");

        if (!_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
        }

        var response = await _httpClient.DeleteAsync($"/users/{User.UserName}");

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/");
        }

        StateHasChanged();

        await loadingIndicator.Hide();
    }
}
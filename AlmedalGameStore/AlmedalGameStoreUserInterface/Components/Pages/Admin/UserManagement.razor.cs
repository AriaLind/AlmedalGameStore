using System.Security.Claims;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using AlmedalGameStoreShared.Dtos.Users;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreUserInterface.Auth;
using Amazon.Runtime;
using Microsoft.AspNetCore.Components;




namespace AlmedalGameStoreUserInterface.Components.Pages.Admin;

public partial class UserManagement
{
    [Inject] SqlUnitOfWork _sqlUnitOfWork { get; set; }

    public List<User> Users { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("AlmedalGameStoreSqlApi");

        var key = await _sqlUnitOfWork.AuthKeyRepository.GetByNameAsync("SqlApi");

        if (!httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", key.Key);
        }

        var response = await httpClient.GetAsync("users");

        var result = await response.Content.ReadFromJsonAsync<UsersList>();
        Users = result.Users.ToList();
       
        
    }
}
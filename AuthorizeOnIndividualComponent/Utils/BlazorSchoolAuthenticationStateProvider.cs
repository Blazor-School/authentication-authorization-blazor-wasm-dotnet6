using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthorizeOnIndividualComponent.Utils;

public class BlazorSchoolAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationDataMemoryStorage _authenticationDataMemoryStorage;

    public BlazorSchoolAuthenticationStateProvider(HttpClient httpClient, AuthenticationDataMemoryStorage authenticationDataMemoryStorage)
    {
        _httpClient = httpClient;
        _authenticationDataMemoryStorage = authenticationDataMemoryStorage;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(_authenticationDataMemoryStorage.Token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(_authenticationDataMemoryStorage.Token);
            identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Blazor School");
        }

        var principal = new ClaimsPrincipal(identity);
        var authenticationState = new AuthenticationState(principal);
        var authenticationTask = Task.FromResult(authenticationState);

        return authenticationTask;
    }

    public async Task LoginAsync()
    {
        string token = await _httpClient.GetStringAsync("example-data/token.json");
        _authenticationDataMemoryStorage.Token = token;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void Logout()
    {
        _authenticationDataMemoryStorage.Token = "";
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

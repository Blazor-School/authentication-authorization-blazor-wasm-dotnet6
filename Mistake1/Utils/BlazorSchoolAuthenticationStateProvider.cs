using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Mistake1.Utils;

public class BlazorSchoolAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
        };

        // Wrong
        var identity = new ClaimsIdentity(claims);

        // Correct
        // var identity = new ClaimsIdentity(claims, "Blazor School");

        var principal = new ClaimsPrincipal(identity);
        var authenticationState = new AuthenticationState(principal);
        var authenticationTask = Task.FromResult(authenticationState);

        return authenticationTask;
    }
}

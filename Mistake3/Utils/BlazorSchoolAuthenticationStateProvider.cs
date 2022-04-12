using Microsoft.AspNetCore.Components.Authorization;
using Mistake3.TransferServices;
using System.Security.Claims;

namespace Mistake3.Utils;

public class BlazorSchoolAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private ClaimsPrincipal _claimsPrincipal = new();
    private readonly MessagingTransferService _messagingTransferService;

    public string Username { get; set; } = "";

    public BlazorSchoolAuthenticationStateProvider(MessagingTransferService messagingTransferService)
    {
        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _messagingTransferService = messagingTransferService;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authenticationState = await task;

        if (authenticationState is not null)
        {
            Username = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _messagingTransferService.Counter++;
        Login(false);
        var authenticationState = new AuthenticationState(_claimsPrincipal);
        var authenticationTask = Task.FromResult(authenticationState);

        return authenticationTask;
    }

    public void Login(bool notify)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,"Blazor School")
        };

        var identity = new ClaimsIdentity(claims, "Blazor School");
        _claimsPrincipal = new ClaimsPrincipal(identity);

        if (notify)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChanged;
}

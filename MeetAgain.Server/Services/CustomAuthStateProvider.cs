using System.Security.Claims;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace MeetAgain.Server.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private const string TokenKey = "authToken";
        private readonly IJSRuntime _js;
        private string? _token;

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetTokenAsync(string? token)
        {
            _token = token;

            if (string.IsNullOrWhiteSpace(token))
                await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            else
                await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_token))
                {
                    // Read from shared localStorage — works across all tabs
                    _token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
                }

                Console.WriteLine($"AuthStateProvider: token is {(string.IsNullOrWhiteSpace(_token) ? "null/empty" : "present")}");

                if (string.IsNullOrWhiteSpace(_token))
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(_token);

                decoded.Claims.TryGetValue("email", out var emailObj);
                string email = emailObj?.ToString() ?? string.Empty;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, decoded.Uid ?? string.Empty),
                    new Claim(ClaimTypes.Email, email)
                };

                var identity = new ClaimsIdentity(claims, "firebase");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                _token = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
using System.Security.Claims;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace MeetAgain.Server.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private string? _token;

        public Task SetTokenAsync(string? token)
        {
            _token = token;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (string.IsNullOrWhiteSpace(_token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            try
            {
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
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}

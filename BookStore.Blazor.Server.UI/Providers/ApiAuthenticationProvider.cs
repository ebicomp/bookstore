using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.Blazor.Server.UI.Providers
{
	public class ApiAuthenticationProvider :AuthenticationStateProvider
	{
        private readonly ILocalStorageService localStorage;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public ApiAuthenticationProvider(ILocalStorageService localStorage)
		{
            this.localStorage = localStorage;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity());
            var token = await localStorage.GetItemAsync<string>("accessToken");
            if (token == null)
            {
                return new AuthenticationState(user);
            }

            var tokens = jwtSecurityTokenHandler.ReadJwtToken(token);
            if (tokens.ValidTo < DateTime.Now)
            {
                return new AuthenticationState(user);
            }
            var claims = tokens.Claims;

            user = new ClaimsPrincipal(new ClaimsIdentity(claims , "jwt"));
            return new AuthenticationState(user);

        }
        public async Task LoggedIn()
        {
            var savedToken = await localStorage.GetItemAsync<string>("accessToken");
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims;
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims , "jwt"));
            claims.ToList().Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);



        }
        public async Task LogedOut()
        {
            await localStorage.RemoveItemAsync("accessToken");
            var nobody = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);


        }
    }
}


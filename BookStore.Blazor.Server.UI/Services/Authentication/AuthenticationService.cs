using System;
using Blazored.LocalStorage;
using BookStore.Blazor.Server.UI.Providers;
using BookStore.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.Blazor.Server.UI.Services.Authentication
{
	public class AuthenticationService:IAuthenticationService
	{
        private readonly IClient client;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(IClient client,
            ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider)
		{
            this.client = client;
            this.localStorageService = localStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsyn(LoginUserDto userDto)
        {
            var response = await client.LoginAsync(userDto);
            await localStorageService.SetItemAsStringAsync("accessToken", response.Token);
            await ((ApiAuthenticationProvider)authenticationStateProvider).LoggedIn();
            return true;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationProvider)authenticationStateProvider).LogedOut();
        }
    }
}


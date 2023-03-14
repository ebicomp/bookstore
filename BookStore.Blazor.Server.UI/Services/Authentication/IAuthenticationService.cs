using System;
using BookStore.Blazor.Server.UI.Services.Base;

namespace BookStore.Blazor.Server.UI.Services.Authentication
{
	public interface IAuthenticationService
	{
		Task<bool> AuthenticateAsyn(LoginUserDto userDto);
		Task Logout();
	}
}


using System;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Users
{
	public class UserDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }

		public string UserRole { get; set; }
	}
}


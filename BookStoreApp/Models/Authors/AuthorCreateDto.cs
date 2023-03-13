﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Authors
{
	public class AuthorCreateDto
	{
		public AuthorCreateDto()
		{
		}
		[Required]
		[StringLength(50)]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
    }
}


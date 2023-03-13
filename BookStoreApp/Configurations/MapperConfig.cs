using System;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Authors;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Models.Users;

namespace BookStoreApp.API.Configurations
{
	public class MapperConfig:Profile
	{
		public MapperConfig()
		{
			CreateMap<AuthorCreateDto, Author>().ReverseMap();
			CreateMap<AuthorUpdateDto, Author>().ReverseMap();
			CreateMap<AuthorReadonlyDto, Author>().ReverseMap();


			CreateMap<Book, ReadOnlyBookDto>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(q => $"{q.Author.FirstName} {q.Author.LastName}" ))
				.ReverseMap();
			CreateMap<CreateBookDto, Book>().ReverseMap();
			CreateMap<UpdateBook, Book>().ReverseMap();

			CreateMap<ApiUser, UserDto>().ReverseMap();
        }

	}
}


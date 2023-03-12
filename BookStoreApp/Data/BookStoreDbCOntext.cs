using System;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data
{
	public class BookStoreDbContext:DbContext
	{
        public BookStoreDbContext(DbContextOptions options) : base(options)
		{

        }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
	}
}


using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data
{
	public class BookStoreDbContext:IdentityDbContext<ApiUser>
	{
        public BookStoreDbContext(DbContextOptions options) : base(options)
		{

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                     Id = "5b12f593-4697-4f2a-afad-9524f894cefd",
                     Name = "User",
                     NormalizedName = "USER"
                },
                new IdentityRole
                {
                     Id = "575b0447-b092-4397-a528-22e400fd0718",
                     Name = "Administrator",
                     NormalizedName = "ADMINISTRATOR"
                }

            );

            var hasher = new PasswordHasher<ApiUser>();
            builder.Entity<ApiUser>().HasData
                (
                new ApiUser
                {
                    Id = "56fa0eef-e65b-49c3-8cca-080b27774a67",
                    FirstName = "John",
                    LastName = "McCartey",
                    Email = "user@bookstore.com",
                    NormalizedEmail = "USER@BOOKSTORE.COM",
                    UserName = "user@bookstore.com",
                    PasswordHash = hasher.HashPassword(null, "p@ssword1")
                },
                new ApiUser
                {
                    Id = "17d35459-b5b8-4ac2-90c8-f3161035524d",
                    FirstName = "admin",
                    LastName = "McCartey",
                    Email = "admin@bookstore.com",
                    NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                    UserName = "admin@bookstore.com",
                    PasswordHash = hasher.HashPassword(null, "p@ssword1")
                }
                );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> {
                     RoleId = "5b12f593-4697-4f2a-afad-9524f894cefd",
                     UserId = "56fa0eef-e65b-49c3-8cca-080b27774a67"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "575b0447-b092-4397-a528-22e400fd0718",
                    UserId = "17d35459-b5b8-4ac2-90c8-f3161035524d"
                }
                );

            base.OnModelCreating(builder);

        }
        public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
	}
}


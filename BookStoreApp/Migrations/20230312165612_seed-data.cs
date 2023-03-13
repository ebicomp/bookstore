using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "575b0447-b092-4397-a528-22e400fd0718", null, "Administrator", "ADMINISTRATOR" },
                    { "5b12f593-4697-4f2a-afad-9524f894cefd", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "17d35459-b5b8-4ac2-90c8-f3161035524d", 0, "38b0ea43-cfeb-4b51-aaf4-a5077d9e92e6", "admin@bookstore.com", false, "admin", "McCartey", false, null, "ADMIN@BOOKSTORE.COM", null, "AQAAAAIAAYagAAAAEJk4+xj2OvP1C/Z146P2K+gBrk542eDb8U106picyBD+lFFekdGtHQyx22oSJ1LxMw==", null, false, "eb157319-bb06-4cdc-9f9d-d52fd07c163d", false, "admin@bookstore.com" },
                    { "56fa0eef-e65b-49c3-8cca-080b27774a67", 0, "6c3ac310-4e9c-4576-8d24-a61d129f900d", "user@bookstore.com", false, "John", "McCartey", false, null, "USER@BOOKSTORE.COM", null, "AQAAAAIAAYagAAAAELHttCauH18yMUob1qKYk/Ik+bqfuX4S6Vxejek8/mB34Kgo8bnna4lqCgNO336nPQ==", null, false, "9da40d1a-0163-4c27-bf52-e43d3707304b", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "575b0447-b092-4397-a528-22e400fd0718", "17d35459-b5b8-4ac2-90c8-f3161035524d" },
                    { "5b12f593-4697-4f2a-afad-9524f894cefd", "56fa0eef-e65b-49c3-8cca-080b27774a67" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "575b0447-b092-4397-a528-22e400fd0718", "17d35459-b5b8-4ac2-90c8-f3161035524d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5b12f593-4697-4f2a-afad-9524f894cefd", "56fa0eef-e65b-49c3-8cca-080b27774a67" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "575b0447-b092-4397-a528-22e400fd0718");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b12f593-4697-4f2a-afad-9524f894cefd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "17d35459-b5b8-4ac2-90c8-f3161035524d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56fa0eef-e65b-49c3-8cca-080b27774a67");
        }
    }
}

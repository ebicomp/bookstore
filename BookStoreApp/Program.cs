using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((cnx, lc)=> 
    lc.WriteTo.Console()
    .ReadFrom.Configuration(cnx.Configuration));

builder.Services.AddDbContext<BookStoreDbCOntext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreConnection")));



builder.Services.AddCors(options => {
    options.AddPolicy("CORS", builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CORS");

app.UseAuthorization();

app.MapControllers();

app.Run();


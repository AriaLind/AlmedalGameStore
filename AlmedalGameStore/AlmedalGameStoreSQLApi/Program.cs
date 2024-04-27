using AlmedalGameStoreDataAccess;
using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints.Swagger;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using AlmedalGameStoreShared.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SqlDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SqlUnitOfWork>();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddUserManager<UserManager<User>>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SqlDbContext>();

builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.DocumentName = $"Magnus API V{s.Version}";
        s.Title = "WebDevelopment Assignment 2 Api";
        s.Description = "Api for handling customers, orders and products.";
        s.Version = "1.0";
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
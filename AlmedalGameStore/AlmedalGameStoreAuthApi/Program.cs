using AlmedalGameStoreDataAccess;
using AlmedalGameStoreShared.Entities;
using FastEndpoints.Swagger;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AlmedalGameStoreDataAccess.UnitsOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SqlDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SqlUnitOfWork>();

builder.Services.AddHttpClient("AlmedalGameStoreSqlApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7237");
});

builder.Services.AddHttpClient("AlmedalGameStoreMongoDbApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7206");
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
// Configure the HTTP request pipeline.

builder.Services.AddIdentityApiEndpoints<User>()
    .AddUserManager<UserManager<User>>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SqlDbContext>();

builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = s =>
    {
        s.DocumentName = $"Auth API V{s.Version}";
        s.Title = "WebDevelopment Assignment 2 Api";
        s.Description = "Api for handling customers, orders and products.";
        s.Version = "1.0";
    };
});


// Add services to the container.

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.MapIdentityApi<User>();

app.Run();


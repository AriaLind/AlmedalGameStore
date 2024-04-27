using AlmedalGameStoreDataAccess.UnitsOfWork;
using FastEndpoints.Swagger;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<MongoDbUnitOfWork>();

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

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();

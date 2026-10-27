using ClothingStoreAPI.Data;
using ClothingStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProductService, ProductService>(); // Dependency injection of the product service
builder.Services.AddScoped<IOrderService, OrderService>(); // Dependency injection of the order service

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}
else
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "Clothing Store API is running");

app.Run();
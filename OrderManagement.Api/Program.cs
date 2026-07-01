using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Middleware;
using OrderManagement.Application.Orders;
using OrderManagement.Application.Orders.Detail;
using OrderManagement.Application.Orders.Search;
using OrderManagement.Application.Users;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Products;
using OrderManagement.Domain.Users;
using OrderManagement.Infrastructure.Orders;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Persistence.Seed;
using OrderManagement.Infrastructure.Products;
using OrderManagement.Infrastructure.Users;

var builder = WebApplication.CreateBuilder(args);

var signingKey = builder.Configuration["Jwt:SigningKey"]!;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = signingKey);
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(settings =>
{
    settings.DocumentSettings = generatorSettings =>
    {
        generatorSettings.Title = "Order Management Service";
        generatorSettings.DocumentName = "v1";
        generatorSettings.Version = "v1";
    };
    settings.EnableJWTBearerAuth = true;
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderSearchQuery, OrderSearchQuery>();
builder.Services.AddScoped<IOrderDetailQuery, OrderDetailQuery>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config => { config.Endpoints.RoutePrefix = "api"; });

app.UseOpenApi();
app.UseSwaggerUi(s => s.ConfigureDefaults());

app.Run();

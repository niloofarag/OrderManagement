using Bogus;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Users;
using OrderManagement.Domain.Products;
using OrderManagement.Domain.Users;

namespace OrderManagement.Infrastructure.Persistence.Seed;

public class DataSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(AppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _dbContext.Users.IgnoreQueryFilters().AnyAsync(cancellationToken))
            return;

        var passwordHash = _passwordHasher.Hash("123456");

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("09#########"))
            .RuleFor(u => u.PasswordHash, _ => passwordHash)
            .RuleFor(u => u.Role, _ => UserRole.User);

        var users = userFaker.Generate(50);

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FullName = "System Admin",
            PhoneNumber = "09000000000",
            PasswordHash = passwordHash,
            Role = UserRole.Admin
        };

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(10, 500));

        var products = productFaker.Generate(200);

        await _dbContext.Users.AddRangeAsync(users.Append(admin), cancellationToken);
        await _dbContext.Products.AddRangeAsync(products, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

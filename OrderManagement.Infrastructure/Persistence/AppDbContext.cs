using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Products;
using OrderManagement.Domain.Users;

namespace OrderManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.StockQuantity).IsRequired();
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId).IsRequired();
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(o => o.CreatedAt).IsRequired();
            builder.Property(o => o.IsDeleted).IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);
        });

        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("order_items");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
        });

        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}

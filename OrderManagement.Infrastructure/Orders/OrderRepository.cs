using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Products;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, IEnumerable<Product> productsToUpdate, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        _dbContext.Products.UpdateRange(productsToUpdate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BulkInsertAsync(List<Order> orders, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertAsync(orders, cancellationToken: cancellationToken);

        var orderItems = orders.SelectMany(o => o.OrderItems).ToList();
        if (orderItems.Count > 0)
            await _dbContext.BulkInsertAsync(orderItems, cancellationToken: cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Orders.Detail;
using OrderManagement.Domain.Orders;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Orders;

public class OrderDetailQuery : IOrderDetailQuery
{
    private readonly AppDbContext _dbContext;

    public OrderDetailQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}

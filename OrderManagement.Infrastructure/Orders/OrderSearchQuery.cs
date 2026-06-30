using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Orders.Search;
using OrderManagement.Domain.Orders;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Orders;

public class OrderSearchQuery : IOrderSearchQuery
{
    private readonly AppDbContext _dbContext;

    public OrderSearchQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Order>> SearchAsync(Guid? customerId, OrderStatus? status,
        DateTime? fromDate, DateTime? toDate, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Orders.AsNoTracking().AsQueryable();

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}

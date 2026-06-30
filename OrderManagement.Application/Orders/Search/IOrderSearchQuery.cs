using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Search;

public interface IOrderSearchQuery
{
    Task<PagedResult<Order>> SearchAsync(Guid? customerId, OrderStatus? status, DateTime? fromDate,
        DateTime? toDate, int page, int pageSize, CancellationToken cancellationToken = default);
}

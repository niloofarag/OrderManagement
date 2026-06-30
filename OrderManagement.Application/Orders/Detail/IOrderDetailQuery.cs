using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Detail;

public interface IOrderDetailQuery
{
    Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

namespace OrderManagement.Domain.Orders;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Guid customerId, IReadOnlyCollection<CreateOrderItemInput> items,
        CancellationToken cancellationToken = default);

    Task<Order> MoveToNextStatusAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(List<Order> orders, CancellationToken cancellationToken = default);
}

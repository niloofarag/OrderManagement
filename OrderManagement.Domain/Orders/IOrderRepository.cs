using OrderManagement.Domain.Products;

namespace OrderManagement.Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, IEnumerable<Product> productsToUpdate, CancellationToken cancellationToken = default);

    Task<Order?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

    Task BulkInsertAsync(List<Order> orders, CancellationToken cancellationToken = default);
}

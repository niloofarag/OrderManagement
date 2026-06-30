using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Orders.Exceptions;
using OrderManagement.Domain.Products;
using OrderManagement.Domain.Products.Exceptions;

namespace OrderManagement.Application.Orders;

public class OrderService : IOrderService
{
    private static readonly SemaphoreSlim StockLock = new(1, 1);

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Order> CreateOrderAsync(Guid customerId, IReadOnlyCollection<CreateOrderItemInput> items,
        CancellationToken cancellationToken = default)
    {
        await StockLock.WaitAsync(cancellationToken);
        try
        {
            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _productRepository.FindByIdsAsync(productIds, cancellationToken);

            var orderItems = new List<OrderItem>();
            var totalAmount = 0m;

            foreach (var item in items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product is null)
                    throw new ProductNotFoundException(item.ProductId);

                if (product.StockQuantity < item.Quantity)
                    throw new InsufficientStockException(item.ProductId);

                product.StockQuantity -= item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                totalAmount += product.Price * item.Quantity;
            }

            var order = new Order
            {
                CustomerId = customerId,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            await _orderRepository.AddAsync(order, products, cancellationToken);

            return order;
        }
        finally
        {
            StockLock.Release();
        }
    }

    public async Task<Order> MoveToNextStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.FindByIdAsync(id, cancellationToken);
        if (order is null)
            throw new OrderNotFoundException(id);

        order.MoveToNextStatus();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.FindByIdAsync(id, cancellationToken);
        if (order is null)
            throw new OrderNotFoundException(id);

        order.IsDeleted = true;
        await _orderRepository.UpdateAsync(order, cancellationToken);
    }

    public Task BulkInsertAsync(List<Order> orders, CancellationToken cancellationToken = default)
    {
        foreach (var order in orders)
        {
            order.Id = Guid.NewGuid();
            foreach (var item in order.OrderItems)
            {
                item.Id = Guid.NewGuid();
                item.OrderId = order.Id;
            }
        }

        return _orderRepository.BulkInsertAsync(orders, cancellationToken);
    }
}

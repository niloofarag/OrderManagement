using OrderManagement.Domain.Common;
using OrderManagement.Domain.Products;

namespace OrderManagement.Domain.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }

    public Order Order { get; set; } = default!;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}

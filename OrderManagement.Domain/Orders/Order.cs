using OrderManagement.Domain.Common;
using OrderManagement.Domain.Orders.Exceptions;

namespace OrderManagement.Domain.Orders;

public class Order : BaseEntity
{
    private static readonly Dictionary<OrderStatus, OrderStatus> NextStatusMap = new()
    {
        [OrderStatus.Pending] = OrderStatus.Confirmed,
        [OrderStatus.Confirmed] = OrderStatus.Shipped,
        [OrderStatus.Shipped] = OrderStatus.Delivered
    };

    public Guid CustomerId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime OrderDate { get; set; }=DateTime.Now;

    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

    public void MoveToNextStatus()
    {
        if (!NextStatusMap.TryGetValue(Status, out var nextStatus))
            throw new OrderStatusTransitionException(Status);

        Status = nextStatus;
    }
}

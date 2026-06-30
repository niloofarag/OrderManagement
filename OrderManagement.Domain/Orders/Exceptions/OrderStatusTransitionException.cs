using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Orders.Exceptions;

public class OrderStatusTransitionException : DomainException
{
    public OrderStatusTransitionException(OrderStatus currentStatus)
        : base($"Order in status {currentStatus} has no next status")
    {
    }
}

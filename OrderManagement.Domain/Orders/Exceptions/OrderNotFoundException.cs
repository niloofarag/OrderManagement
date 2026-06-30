using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Orders.Exceptions;

public class OrderNotFoundException : DomainException
{
    public OrderNotFoundException(Guid orderId) : base($"Order {orderId} was not found")
    {
    }
}

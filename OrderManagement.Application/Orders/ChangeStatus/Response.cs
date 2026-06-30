using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.ChangeStatus;

internal sealed record Response
{
    public Guid Id { get; init; }

    public OrderStatus Status { get; init; }
}

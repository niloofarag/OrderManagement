using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Create;

internal sealed record Response
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public OrderStatus Status { get; init; }

    public DateTime OrderDate { get; init; }

    public decimal TotalAmount { get; init; }
}

using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Search;

internal sealed record Request
{
    public Guid? CustomerId { get; init; }

    public OrderStatus? Status { get; init; }

    public DateTime? FromDate { get; init; }

    public DateTime? ToDate { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;
}

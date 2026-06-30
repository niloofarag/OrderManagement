using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Search;

internal sealed record Response
{
    public List<Item> Items { get; init; } = new();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    internal sealed record Item
    {
        public Guid Id { get; init; }

        public Guid CustomerId { get; init; }

        public OrderStatus Status { get; init; }

        public DateTime OrderDate { get; init; }

        public decimal TotalAmount { get; init; }
    }
}

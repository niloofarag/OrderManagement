using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Detail;

internal sealed record Response
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public OrderStatus Status { get; init; }

    public DateTime OrderDate { get; init; }

    public decimal TotalAmount { get; init; }

    public List<Item> Items { get; init; } = new();

    internal sealed record Item
    {
        public Guid ProductId { get; init; }

        public int Quantity { get; init; }

        public decimal UnitPrice { get; init; }
    }
}

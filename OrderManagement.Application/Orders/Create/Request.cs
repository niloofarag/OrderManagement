namespace OrderManagement.Application.Orders.Create;

internal sealed record Request
{
    public Guid CustomerId { get; init; }

    public List<Item> Items { get; init; } = new();

    internal sealed record Item
    {
        public Guid ProductId { get; init; }

        public int Quantity { get; init; }
    }
}

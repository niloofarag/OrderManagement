namespace OrderManagement.Application.Orders.Delete;

internal sealed record Request
{
    public Guid Id { get; init; }
}

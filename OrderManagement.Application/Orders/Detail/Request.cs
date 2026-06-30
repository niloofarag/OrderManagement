namespace OrderManagement.Application.Orders.Detail;

internal sealed record Request
{
    public Guid Id { get; init; }
}

namespace OrderManagement.Application.Orders.ChangeStatus;

internal sealed record Request
{
    public Guid Id { get; init; }
}

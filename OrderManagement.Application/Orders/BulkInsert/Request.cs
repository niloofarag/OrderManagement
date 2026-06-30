using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.BulkInsert;

internal sealed record Request
{
    public List<OrderRequestModel> Orders { get; init; } = new();

 
}
internal sealed record OrderRequestModel
{
    public Guid CustomerId { get; init; }

    public List<ItemRequestModel> Items { get; init; } = new();

    
}
internal sealed record ItemRequestModel
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }
}
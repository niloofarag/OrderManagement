using FastEndpoints;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Users;
using Order = OrderManagement.Domain.Orders.Order;
using OrderItem = OrderManagement.Domain.Orders.OrderItem;

namespace OrderManagement.Application.Orders.BulkInsert;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IOrderService _orderService;

    public Endpoint(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override void Configure()
    {
        Post("/orders/bulk");
        Roles(nameof(UserRole.Admin), nameof(UserRole.User));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var orders = req.Orders.Select(o => new Order()
        {
            CustomerId = o.CustomerId,
            TotalAmount = o.Items.Sum(i => i.UnitPrice * i.Quantity),
            OrderItems = o.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        }).ToList();

        await _orderService.BulkInsertAsync(orders, ct);
        await Send.OkAsync(ct);
    }
}

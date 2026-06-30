using FastEndpoints;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Orders.Create;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IOrderService _orderService;

    public Endpoint(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override void Configure()
    {
        Post("/orders");
        Roles(nameof(UserRole.Admin), nameof(UserRole.User));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var items = req.Items.Select(i => new CreateOrderItemInput(i.ProductId, i.Quantity)).ToList();
        var order = await _orderService.CreateOrderAsync(req.CustomerId, items, ct);

        await Send.OkAsync(new Response
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount
        }, ct);
    }
}

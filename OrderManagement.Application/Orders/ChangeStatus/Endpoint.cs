using FastEndpoints;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Orders.ChangeStatus;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IOrderService _orderService;

    public Endpoint(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override void Configure()
    {
        Post("/orders/{id}/next-status");
        Roles(nameof(UserRole.Admin), nameof(UserRole.User));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var order = await _orderService.MoveToNextStatusAsync(req.Id, ct);

        await Send.OkAsync(new Response
        {
            Id = order.Id,
            Status = order.Status
        }, ct);
    }
}

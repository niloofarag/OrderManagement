using FastEndpoints;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Orders.Delete;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IOrderService _orderService;

    public Endpoint(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override void Configure()
    {
        Delete("/orders/{id}");
        Roles(nameof(UserRole.Admin));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _orderService.DeleteAsync(req.Id, ct);
        await Send.NoContentAsync(ct);
    }
}

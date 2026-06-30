using FastEndpoints;
using OrderManagement.Domain.Orders.Exceptions;
using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Orders.Detail;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IOrderDetailQuery _orderDetailQuery;

    public Endpoint(IOrderDetailQuery orderDetailQuery)
    {
        _orderDetailQuery = orderDetailQuery;
    }

    public override void Configure()
    {
        Get("/orders/{id}");
        Roles(nameof(UserRole.Admin), nameof(UserRole.User));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var order = await _orderDetailQuery.FindByIdAsync(req.Id, ct);
        if (order is null)
            throw new OrderNotFoundException(req.Id);

        await Send.OkAsync(new Response
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(i => new Response.Item
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        }, ct);
    }
}

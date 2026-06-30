using FastEndpoints;
using OrderManagement.Domain.Users;

namespace OrderManagement.Application.Orders.Search;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IOrderSearchQuery _orderSearchQuery;

    public Endpoint(IOrderSearchQuery orderSearchQuery)
    {
        _orderSearchQuery = orderSearchQuery;
    }

    public override void Configure()
    {
        Get("/orders");
        Roles(nameof(UserRole.Admin), nameof(UserRole.User));
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _orderSearchQuery.SearchAsync(req.CustomerId, req.Status, req.FromDate,
            req.ToDate, req.Page, req.PageSize, ct);

        await Send.OkAsync(new Response
        {
            Items = result.Items.Select(o => new Response.Item
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            }).ToList(),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        }, ct);
    }
}

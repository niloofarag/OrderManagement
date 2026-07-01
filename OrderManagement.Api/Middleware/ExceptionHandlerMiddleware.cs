using System.Text.Json;
using OrderManagement.Domain.Common;
using OrderManagement.Domain.Orders.Exceptions;
using OrderManagement.Domain.Products.Exceptions;
using OrderManagement.Domain.Users.Exceptions;

namespace OrderManagement.Api.Middleware;

public sealed class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = exception switch
            {
                OrderNotFoundException or ProductNotFoundException => StatusCodes.Status404NotFound,
                InsufficientStockException or OrderStatusTransitionException => StatusCodes.Status409Conflict,
                InvalidCredentialsException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            await response.WriteAsync(JsonSerializer.Serialize(new { message = exception.Message }));
        }
    }
}

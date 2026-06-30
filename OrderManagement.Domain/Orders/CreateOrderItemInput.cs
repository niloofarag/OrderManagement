namespace OrderManagement.Domain.Orders;

public record CreateOrderItemInput(Guid ProductId, int Quantity);

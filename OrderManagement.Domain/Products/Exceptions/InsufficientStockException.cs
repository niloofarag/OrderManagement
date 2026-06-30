using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Products.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(Guid productId) : base($"Product {productId} does not have enough stock")
    {
    }
}

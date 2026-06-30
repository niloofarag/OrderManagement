using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Products.Exceptions;

public class ProductNotFoundException : DomainException
{
    public ProductNotFoundException(Guid productId) : base($"Product {productId} was not found")
    {
    }
}

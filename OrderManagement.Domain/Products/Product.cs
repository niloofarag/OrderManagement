using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }
}

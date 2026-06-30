namespace OrderManagement.Domain.Products;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Product>> FindByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Products;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Products;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Product>> FindByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);
    }
}

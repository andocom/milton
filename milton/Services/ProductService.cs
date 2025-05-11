using Microsoft.EntityFrameworkCore;
using milton.Data;

public class ProductService
{
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db) => _db = db;

    public async Task<List<Product>> GetAllAsync() =>
        await _db.Products.OrderBy(p => p.Name).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _db.Products.FindAsync(id);

    public async Task<Product?> GetBySkuAsync(string sku) =>
        await _db.Products.FirstOrDefaultAsync(p => p.Sku == sku);

    public async Task AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<Product> products)
    {
        _db.Products.AddRange(products);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> GetActiveAsync() =>
        await _db.Products.Where(p => p.Active).OrderBy(p => p.Name).ToListAsync();
}

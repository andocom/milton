using Microsoft.EntityFrameworkCore;
using milton.Models.CompetitorPrices;

namespace milton.Data
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db) => _db = db;

        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Products
                .Include(ps => ps.ProductSnapshots)
                .Include(ps => ps.CompetitorProduct)
                .ToListAsync();
        }


        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
                .Include(ps => ps.ProductSnapshots)
                .Include(ps => ps.CompetitorProduct)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await _db.Products
                .Include(ps => ps.ProductSnapshots)
                .Include(ps => ps.CompetitorProduct)
                .FirstOrDefaultAsync(ps => ps.SKU == sku);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
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
}



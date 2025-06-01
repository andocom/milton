using milton.Models.CompetitorPrices;
using Microsoft.EntityFrameworkCore;

namespace milton.Data
{
    public class CompetitorProductService
    {
        private readonly ApplicationDbContext _db;

        public CompetitorProductService(ApplicationDbContext db) => _db = db;

        // Create
        public async Task<CompetitorProduct> AddAsync(CompetitorProduct competitorProduct)
        {
            _db.CompetitorProducts.Add(competitorProduct);
            await _db.SaveChangesAsync();
            return competitorProduct;
        }

        // Read All
        public async Task<List<CompetitorProduct>> GetAllAsync()
        {
            return await _db.CompetitorProducts
                .Include(ps => ps.Product)
                .Include(ps => ps.Competitor)
                .ToListAsync();
        }

        // Read by Id
        public async Task<CompetitorProduct?> GetByIdAsync(int id)
        {
            return await _db.CompetitorProducts
                .Include(ps => ps.Product)
                .Include(ps => ps.Competitor)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task UpsertAsync(int productId, int competitorId, string sku)
        {
            var existing = await _db.CompetitorProducts
                .FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.CompetitorId == competitorId);

            if (existing != null)
            {
                if (existing.CompetitorSKU != sku)
                {
                    existing.CompetitorSKU = sku;
                    _db.CompetitorProducts.Update(existing);
                }
            }
            else
            {
                var newRecord = new CompetitorProduct
                {
                    ProductId = productId,
                    CompetitorId = competitorId,
                    CompetitorSKU = sku
                };
                _db.CompetitorProducts.Add(newRecord);
            }

            await _db.SaveChangesAsync();
        }


        // Update
        public async Task<bool> UpdateAsync(CompetitorProduct updated)
        {
            var existing = await _db.CompetitorProducts.FindAsync(updated.Id);
            if (existing == null) return false;

            existing.ProductId = updated.ProductId;
            existing.CompetitorId = updated.CompetitorId;

            await _db.SaveChangesAsync();
            return true;
        }

        // Delete
        public async Task DeleteAsync(int id)
        {
            var competitorProduct = await _db.CompetitorProducts.FindAsync(id);
            if (competitorProduct != null)
            {
                _db.CompetitorProducts.Remove(competitorProduct);
                await _db.SaveChangesAsync();
            }
        }

    }

}



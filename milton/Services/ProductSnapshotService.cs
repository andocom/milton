using milton.Models.CompetitorPrices;
using Microsoft.EntityFrameworkCore;

namespace milton.Data
{
    public class ProductSnapshotService
    {
        private readonly ApplicationDbContext _db;

        public ProductSnapshotService(ApplicationDbContext db) => _db = db;

        // Create
        public async Task<ProductSnapshot> AddAsync(ProductSnapshot snapshot)
        {
            _db.ProductSnapshots.Add(snapshot);
            await _db.SaveChangesAsync();
            return snapshot;
        }

        // Read All
        public async Task<List<ProductSnapshot>> GetAllAsync()
        {
            return await _db.ProductSnapshots
                .Include(ps => ps.Product)
                .Include(ps => ps.Competitor)
                .Include(ps => ps.Snapshot)
                .ToListAsync();
        }

        // Read by Id
        public async Task<ProductSnapshot?> GetByIdAsync(int id)
        {
            return await _db.ProductSnapshots
                .Include(ps => ps.Product)
                .Include(ps => ps.Competitor)
                .Include(ps => ps.Snapshot)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        // Update
        public async Task<bool> UpdateAsync(ProductSnapshot updated)
        {
            var existing = await _db.ProductSnapshots.FindAsync(updated.Id);
            if (existing == null) return false;

            existing.ProductId = updated.ProductId;
            existing.CompetitorId = updated.CompetitorId;
            existing.SnapshotId = updated.SnapshotId;
            existing.Price = updated.Price;

            await _db.SaveChangesAsync();
            return true;
        }

        // Delete
        public async Task DeleteAsync(int id)
        {
            var productSnapshot = await _db.ProductSnapshots.FindAsync(id);
            if (productSnapshot != null)
            {
                _db.ProductSnapshots.Remove(productSnapshot);
                await _db.SaveChangesAsync();
            }
        }

    }

}



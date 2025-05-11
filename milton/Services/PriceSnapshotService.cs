using Microsoft.EntityFrameworkCore;
using milton.Data;

public class PriceSnapshotService
{
    private readonly ApplicationDbContext _db;

    public PriceSnapshotService(ApplicationDbContext db) => _db = db;

    public async Task<List<PriceSnapshot>> GetAllAsync() =>
        await _db.PriceSnapshots
            .Include(p => p.Product)
            .Include(p => p.PriceSource)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(PriceSnapshot snapshot)
    {
        _db.PriceSnapshots.Add(snapshot);
        await _db.SaveChangesAsync();
    }

    public async Task<List<PriceSnapshot>> GetByProductIdAsync(int productId) =>
        await _db.PriceSnapshots
            .Where(p => p.ProductId == productId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<List<PriceSnapshot>> GetByPriceSourceIdAsync(int priceSourceId) =>
        await _db.PriceSnapshots
            .Where(p => p.PriceSourceId == priceSourceId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
}

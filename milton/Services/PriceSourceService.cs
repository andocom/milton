using Microsoft.EntityFrameworkCore;
using milton.Data;

public class PriceSourceService
{
    private readonly ApplicationDbContext _db;

    public PriceSourceService(ApplicationDbContext db) => _db = db;

    public async Task<List<PriceSource>> GetAllAsync() =>
        await _db.PriceSources.OrderBy(s => s.Name).ToListAsync();

    public async Task<PriceSource?> GetByIdAsync(int id) =>
        await _db.PriceSources.FindAsync(id);

    public async Task AddRangeAsync(List<PriceSource> priceSources)
    {
        _db.PriceSources.AddRange(priceSources);
        await _db.SaveChangesAsync();
    }

    public async Task AddAsync(PriceSource priceSource)
    {
        _db.PriceSources.Add(priceSource);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(PriceSource priceSource)
    {
        _db.PriceSources.Update(priceSource);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var priceSource = await _db.PriceSources.FindAsync(id);
        if (priceSource != null)
        {
            _db.PriceSources.Remove(priceSource);
            await _db.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;


public class ProductSnapshotService
{
    private readonly ProductDbContext _db;

    public ProductSnapshotService(ProductDbContext db)
    {
        _db = db;
    }

    public async Task SaveSnapshotsAsync(List<ProductSnapshot> snapshots)
    {
        if (snapshots == null || snapshots.Count == 0)
            return;

        var snapshotDate = snapshots.First().SnapshotDate.Date;
        var source = snapshots.First().Source;

        // Check for existing entries for this source + date + SKU
        var existingKeys = await _db.Snapshots
            .Where(p => p.SnapshotDate.Date == snapshotDate && p.Source == source)
            .Select(p => p.SKU)
            .ToListAsync();

        var newSnapshots = snapshots
            .Where(s => !existingKeys.Contains(s.SKU))
            .ToList();

        if (newSnapshots.Count > 0)
        {
            await _db.Snapshots.AddRangeAsync(newSnapshots);
            await _db.SaveChangesAsync();
        }
    }

}

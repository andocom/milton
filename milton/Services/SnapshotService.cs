using Microsoft.EntityFrameworkCore;
using milton.Models.CompetitorPrices;

namespace milton.Data
{
    public class SnapshotService
    {
        private readonly ApplicationDbContext _db;

        public SnapshotService(ApplicationDbContext db) => _db = db;

        public async Task<List<Snapshot>> GetAllAsync() =>
            await _db.Snapshots.OrderByDescending(p => p.CreatedAt).ToListAsync();

        public async Task<Snapshot?> GetByIdAsync(int id) =>
            await _db.Snapshots.FindAsync(id);

        public async Task<Snapshot> AddAsync(Snapshot Snapshot)
        {
            _db.Snapshots.Add(Snapshot);
            await _db.SaveChangesAsync();
            return Snapshot;
        }

        public async Task<List<Snapshot>> AddRangeAsync(List<Snapshot> Snapshots)
        {
            _db.Snapshots.AddRange(Snapshots);
            await _db.SaveChangesAsync();
            return Snapshots;
        }

        public async Task UpdateAsync(Snapshot Snapshot)
        {
            _db.Snapshots.Update(Snapshot);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Snapshot = await _db.Snapshots.FindAsync(id);
            if (Snapshot != null)
            {
                _db.Snapshots.Remove(Snapshot);
                await _db.SaveChangesAsync();
            }
        }

    }
}



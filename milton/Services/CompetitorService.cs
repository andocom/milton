using Microsoft.EntityFrameworkCore;
using milton.Models.CompetitorPrices;

namespace milton.Data
{
    public class CompetitorService
    {
        private readonly ApplicationDbContext _db;

        public CompetitorService(ApplicationDbContext db) => _db = db;

        public async Task<List<Competitor>> GetAllAsync() =>
            await _db.Competitors.OrderBy(p => p.Name).ToListAsync();

        public async Task<Competitor?> GetByIdAsync(int id) =>
            await _db.Competitors.FindAsync(id);

        public async Task AddAsync(Competitor Competitor)
        {
            _db.Competitors.Add(Competitor);
            await _db.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Competitor> Competitors)
        {
            _db.Competitors.AddRange(Competitors);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Competitor Competitor)
        {
            _db.Competitors.Update(Competitor);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Competitor = await _db.Competitors.FindAsync(id);
            if (Competitor != null)
            {
                _db.Competitors.Remove(Competitor);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Competitor>> GetActiveAsync() =>
            await _db.Competitors.Where(p => p.Active).OrderBy(p => p.Name).ToListAsync();
    }
}



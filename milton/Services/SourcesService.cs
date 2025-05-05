using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SourcesService
{
    private readonly ProductDbContext _db;

    public SourcesService(ProductDbContext db)
    {
        _db = db;
    }

    public async Task<List<ScrapeSource>> GetAllAsync()
    {
        return await _db.ScrapeSources
            .OrderBy(s => s.SourceName)
            .ToListAsync();
    }

    public async Task<List<string>> GetUrlsBySourceAsync(string sourceName)
    {
        return await _db.ScrapeSources
            .Where(s => s.SourceName == sourceName && s.Active)
            .Select(s => s.Url)
            .ToListAsync();
    }

    public async Task<ScrapeSource?> GetByIdAsync(int id)
    {
        return await _db.ScrapeSources.FindAsync(id);
    }

    public async Task<List<ScrapeSource>> GetBySourceAsync(string sourceName)
    {
        return await _db.ScrapeSources
            .Where(s => s.SourceName == sourceName)
            .ToListAsync();
    }
}

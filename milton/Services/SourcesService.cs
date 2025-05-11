using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using milton.Data;

public class SourcesService
{
    private readonly ApplicationDbContext _db;

    public SourcesService(ApplicationDbContext db)
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

    public async Task AddSource(ScrapeSource source)
    {
        _db.ScrapeSources.Add(source);
        await _db.SaveChangesAsync();
    }

    public async Task AddRange(List<ScrapeSource> sourceList)
    {
        var existingUrls = await _db.ScrapeSources
            .Select(s => s.Url.ToLower())
            .ToListAsync();

        var newSources = sourceList
            .Where(s => !existingUrls.Contains(s.Url.ToLower()))
            .ToList();

        if (newSources.Any())
        {
            _db.ScrapeSources.AddRange(newSources);
            await _db.SaveChangesAsync();
        }
    }


    public async Task DeleteAllSourcesAsync()
    {
        var allSources = await _db.ScrapeSources.ToListAsync();
        _db.ScrapeSources.RemoveRange(allSources);
        await _db.SaveChangesAsync();
    }
}

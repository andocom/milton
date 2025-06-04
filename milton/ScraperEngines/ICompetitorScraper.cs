namespace milton.ScraperEngines
{
    public interface ICompetitorScraper
    {
        string CompetitorName { get; }
        Task<decimal?> GetPriceAsync(string competitorSku);
    }
}
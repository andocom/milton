public class ScrapeSource
{
    public int Id { get; set; }
    public string SourceName { get; set; }   // e.g. "TrophiesDirect"
    public string Url { get; set; }
    public bool Active { get; set; } = true; // allow enabling/disabling
    public string? Category { get; set; }    // optional: "Academic", etc.
}
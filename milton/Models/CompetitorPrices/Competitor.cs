using System.Text.Json.Serialization;

namespace milton.Models.CompetitorPrices
{
    public class Competitor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ProductSnapshot> ProductSnapshots { get; set; }
        public ICollection<CompetitorProduct> CompetitorProduct { get; set; }
        public Competitor()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}

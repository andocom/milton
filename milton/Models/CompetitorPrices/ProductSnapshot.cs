using System.Text.Json.Serialization;

namespace milton.Models.CompetitorPrices
{
    public class ProductSnapshot
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int CompetitorId { get; set; }
        [JsonIgnore]
        public Competitor Competitor { get; set; }
        public int SnapshotId { get; set; }
        [JsonIgnore]
        public Snapshot Snapshot { get; set; }
        public decimal Price { get; set; }
    }
}

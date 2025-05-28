namespace milton.Models.CompetitorPrices
{
    public class ProductSnapshot
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; }
        public int SnapshotId { get; set; }
        public Snapshot Snapshot { get; set; }
        public decimal Price { get; set; }
    }
}

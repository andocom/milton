namespace milton.Models.CompetitorPrices
{
    public class Snapshot
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductSnapshot> ProductSnapshots { get; set; }
        public Snapshot()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}

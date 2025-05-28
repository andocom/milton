namespace milton.Models.CompetitorPrices
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ProductSnapshot> ProductSnapshots { get; set; }
        public ICollection<CompetitorProduct> CompetitorProduct { get; set; }
        public Product()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}

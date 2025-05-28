namespace milton.Models.CompetitorPrices
{
    public class CompetitorProduct
    {
        public int Id { get; set; }
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string CompetitorSKU{ get; set; } = string.Empty;

    }
}

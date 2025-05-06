public class PriceSnapshot
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int PriceSourceId { get; set; }
    public PriceSource PriceSource { get; set; }

    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
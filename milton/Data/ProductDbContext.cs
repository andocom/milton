using Microsoft.EntityFrameworkCore;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    public DbSet<ProductSnapshot> Snapshots { get; set; }
    public DbSet<ScrapeSource> ScrapeSources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSnapshot>(entity =>
        {
            // Index for efficient lookup and duplicate protection
            entity.HasIndex(p => new { p.SKU, p.SnapshotDate, p.Source });

            // Enforce decimal precision for Price
            entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
        });
    }
}

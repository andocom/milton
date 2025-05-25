using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using milton.Models.CompetitorPrices;
using System.Text.Json;

namespace milton.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        //public DbSet<ProductSnapshot> Snapshots { get; set; }
        //public DbSet<ScrapeSource> ScrapeSources { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<PriceSource> PriceSources { get; set; }
        //public DbSet<PriceSnapshot> PriceSnapshots { get; set; }

        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<CompetitorProduct> CompetitorProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSnapshot> ProductSnapshots { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Competitor Prices

            // Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            // Competitor
            modelBuilder.Entity<Competitor>()
                .HasKey(c => c.Id);

            // CompetitorProduct (many-to-one: Product + Competitor)
            modelBuilder.Entity<CompetitorProduct>()
                .HasKey(cp => cp.Id); // or composite key if you prefer (ProductId, CompetitorId)

            modelBuilder.Entity<CompetitorProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CompetitorProduct)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompetitorProduct>()
                .HasOne(cp => cp.Competitor)
                .WithMany(c => c.CompetitorProduct)
                .HasForeignKey(cp => cp.CompetitorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Snapshot
            modelBuilder.Entity<Snapshot>()
                .HasKey(s => s.Id);

            // ProductSnapshot (ties it all together: price at snapshot for product+competitor)
            modelBuilder.Entity<ProductSnapshot>()
                .HasKey(ps => ps.Id);

            modelBuilder.Entity<ProductSnapshot>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSnapshots)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductSnapshot>()
                .HasOne(ps => ps.Competitor)
                .WithMany(c => c.ProductSnapshots)
                .HasForeignKey(ps => ps.CompetitorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductSnapshot>()
                .HasOne(ps => ps.Snapshot)
                .WithMany(s => s.ProductSnapshots)
                .HasForeignKey(ps => ps.SnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: enforce that a ProductSnapshot is unique for a given Product+Competitor+Snapshot combo
            modelBuilder.Entity<ProductSnapshot>()
                .HasIndex(ps => new { ps.ProductId, ps.CompetitorId, ps.SnapshotId })
                .IsUnique();

            modelBuilder.Entity<ProductSnapshot>()
                .Property(ps => ps.Price)
                .HasColumnType("decimal(18,2)");


        }

}


}

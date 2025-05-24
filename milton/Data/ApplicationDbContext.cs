using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace milton.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<ProductSnapshot> Snapshots { get; set; }
        public DbSet<ScrapeSource> ScrapeSources { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<PriceSource> PriceSources { get; set; }
        public DbSet<PriceSnapshot> PriceSnapshots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Product)
            //    .WithMany()
            //    .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<PriceSnapshot>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<PriceSnapshot>()
                .HasOne(p => p.PriceSource)
                .WithMany()
                .HasForeignKey(p => p.PriceSourceId);

            modelBuilder.Entity<ProductSnapshot>(entity =>
            {
                // Index for efficient lookup and duplicate protection
                entity.HasIndex(p => new { p.SKU, p.SnapshotDate, p.Source });

                // Enforce decimal precision for Price
                entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
            });
        }

    }


}

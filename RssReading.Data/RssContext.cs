using Microsoft.EntityFrameworkCore;
using RssReading.Data.Models;

namespace RssReading.Data
{
    public class RssContext : DbContext
    {
        public DbSet<RssItemData> RssItemDatas { get; set; }
        public DbSet<SourceData> SourceDatas { get; set; }

        public RssContext(DbContextOptions<RssContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RssItemData>()
                .HasKey(i => new { i.Title, i.PublishDate });
            modelBuilder.Entity<RssItemData>()
                .HasOne(i => i.Source)
                .WithMany(s => s.RssItems)
                .HasForeignKey(i => i.SourceId);

            modelBuilder.Entity<SourceData>()
                .HasKey(s => new { s.Id });
            modelBuilder.Entity<SourceData>()
                .HasMany(s => s.RssItems)
                .WithOne(i => i.Source)
                .HasForeignKey(i => i.SourceId);
        }
    }
}
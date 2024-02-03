using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using UrlShortener.Entities;
using UrlShortener.UniqueUrlCodesGeneration;

namespace UrlShortener;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
    : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("zaply");

        //modelBuilder.Entity<HistoryRow>().ToTable("__Zaply_MigrationsHistory");

        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);

            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}

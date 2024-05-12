using Microsoft.EntityFrameworkCore;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data;

public class PhraseFinderDbContext : DbContext
{
    public DbSet<Phrase> Phrases { get; set; }
    public DbSet<PhraseDictionary> PhraseDictionaries { get; set; }

    public PhraseFinderDbContext(DbContextOptions<PhraseFinderDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhraseDictionary>()
            .Property(pd => pd.AddedAt)
            .HasDefaultValueSql("Now()");

        modelBuilder.Entity<PhraseDictionary>()
	        .Property(pd => pd.Format)
	        .HasConversion<string>();
    }
}
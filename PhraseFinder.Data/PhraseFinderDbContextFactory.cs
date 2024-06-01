using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PhraseFinder.Data;

public class PhraseFinderDbContextFactory : IDesignTimeDbContextFactory<PhraseFinderDbContext>
{
    public PhraseFinderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PhraseFinderDbContext>();
        optionsBuilder.UseJetOleDb("Data Source=expresiones-y-locuciones.accdb");
        return new PhraseFinderDbContext(optionsBuilder.Options);
    }
}
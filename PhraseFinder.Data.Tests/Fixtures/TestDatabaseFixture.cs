using Microsoft.EntityFrameworkCore;

namespace PhraseFinder.Data.Tests.Fixtures;

public class TestDatabaseFixture
{
    private const string ConnectionString = "Data Source=PhraseFinder.test.accdb";
    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (Lock)
        {
            if (_databaseInitialized)
            {
                return;
            }
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            _databaseInitialized = true;
        }
    }

    public PhraseFinderDbContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PhraseFinderDbContext>()
            .UseJetOleDb(ConnectionString);
        return new PhraseFinderDbContext(optionsBuilder.Options);
    }
}
using PhraseFinderServiceReference;

namespace PhraseFinder.WCF.Tests.Fixtures;

public class ServiceFixture : IDisposable
{
    public IPhraseFinderService Service { get; } = new PhraseFinderServiceClient();

    public void Dispose()
    {
    }
}
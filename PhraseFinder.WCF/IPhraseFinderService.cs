using System.Collections.Generic;
using System.ServiceModel;

namespace PhraseFinder.WCF
{
    [ServiceContract]
    public interface IPhraseFinderService
    {
        [OperationContract]
        IEnumerable<FoundPhrase> FindPhrases(string text);
    }
}

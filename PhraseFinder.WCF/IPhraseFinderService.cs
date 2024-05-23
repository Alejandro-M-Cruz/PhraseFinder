using System.ServiceModel;
using System.Threading.Tasks;
using PhraseFinder.WCF.Contracts;

namespace PhraseFinder.WCF
{
    [ServiceContract]
    public interface IPhraseFinderService
    {
        [OperationContract]
        Task<PhraseAnalysis> FindPhrasesAsync(string text);
    }
}

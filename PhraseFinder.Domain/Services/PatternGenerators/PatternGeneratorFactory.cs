using System.ComponentModel;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.PhraseCleaners;
using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Services.PatternGenerators;

public static class PatternGeneratorFactory
{
    public static IPatternGenerator CreateGenerator(PhraseDictionaryFormat format)
    {
        switch (format)
        {
            case PhraseDictionaryFormat.DleTxt:
                IPhraseSplitter[] splitters = 
                [
                    new TbPhraseSplitter(), 
                    new GenderPhraseSplitter()
                ];
                return new DleTxtPatternGenerator(new DleTxtPhraseCleaner(), splitters);
            default: 
                throw new InvalidEnumArgumentException();
        }
    }
}

using System.ComponentModel;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.PhraseDictionaryReaders;

public static class PhraseDictionaryReaderFactory
{
    public static IPhraseDictionaryReader CreateReader(PhraseDictionaryFormat format, string filePath)
    {
        return format switch
        {
            PhraseDictionaryFormat.DleTxt => new DleTxtPhraseDictionaryReader(filePath),
            _ => throw new InvalidEnumArgumentException(
                nameof(format),
                (int)format,
                typeof(PhraseDictionaryFormat))
        };
    }
}

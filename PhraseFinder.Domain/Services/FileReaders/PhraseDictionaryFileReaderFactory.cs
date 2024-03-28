using System.ComponentModel;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.FileReaders;

public static class PhraseDictionaryFileReaderFactory
{
    public static IPhraseDictionaryFileReader CreateReader(PhraseDictionaryFormat format, string filePath)
    {
        return format switch
        {
            PhraseDictionaryFormat.DleTxt => new DleTxtPhraseDictionaryFileReader(filePath),
            _ => throw new InvalidEnumArgumentException(
	            nameof(format), 
	            (int)format, 
	            typeof(PhraseDictionaryFormat))
        };
    }

}

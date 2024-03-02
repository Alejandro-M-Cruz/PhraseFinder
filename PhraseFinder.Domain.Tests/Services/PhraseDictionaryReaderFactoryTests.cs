using System.ComponentModel;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseDictionaryReaderFactoryTests
{
    [Fact]
    public void CreateReader_WithValidPhraseDictionaryFormat_ReturnsPhraseDictionaryReader()
    {
        var format = PhraseDictionaryFormat.DleTxt;

        var reader = PhraseDictionaryReaderFactory.CreateReader(format, filePath: "example.txt");

        Assert.IsType<DleTxtPhraseDictionaryReader>(reader);
    }
    
    [Fact]
    public void CreateReader_WithUnsupportedDictionaryFormat_ThrowsInvalidEnumArgumentException()
    {
        var format = (PhraseDictionaryFormat)int.MaxValue;

        Assert.Throws<InvalidEnumArgumentException>(() =>
            PhraseDictionaryReaderFactory.CreateReader(format, filePath: "example.txt"));
    }
}
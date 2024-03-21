using System.ComponentModel;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.FileReaders;

namespace PhraseFinder.Domain.Tests.Services.FileReaders;

public class PhraseDictionaryFileReaderFactoryTests
{
    [Fact]
    public void CreateReader_WithValidPhraseDictionaryFormat_ReturnsPhraseDictionaryReader()
    {
        var format = PhraseDictionaryFormat.DleTxt;

        var reader = PhraseDictionaryFileReaderFactory.CreateReader(format, filePath: "example.txt");

        Assert.IsType<DleTxtPhraseDictionaryFileReader>(reader);
    }

    [Fact]
    public void CreateReader_WithUnsupportedDictionaryFormat_ThrowsInvalidEnumArgumentException()
    {
        var format = (PhraseDictionaryFormat)int.MaxValue;

        Assert.Throws<InvalidEnumArgumentException>(() =>
            PhraseDictionaryFileReaderFactory.CreateReader(format, filePath: "example.txt"));
    }
}
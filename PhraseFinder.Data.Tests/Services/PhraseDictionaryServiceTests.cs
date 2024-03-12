using Microsoft.EntityFrameworkCore;
using PhraseFinder.Data.Services;
using PhraseFinder.Data.Tests.Fixtures;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Tests.Services;

public class PhraseDictionaryServiceTests : IDisposable
{
    private readonly PhraseFinderDbContext _dbContext;
    private readonly PhraseDictionaryService _service;

    public PhraseDictionaryServiceTests()
    {
        _dbContext = new TestDatabaseFixture().CreateContext();
        _service = new PhraseDictionaryService(_dbContext);
    }
    
    [Fact]
    public async Task AddPhraseDictionaryAsync_WithValidDictionary_AddsTheDictionaryToDatabase()
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = new string('a', count: 255),
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test.txt"
        };

        await _dbContext.Database.BeginTransactionAsync();
        await _service.AddPhraseDictionaryAsync(phraseDictionary);
        _dbContext.ChangeTracker.Clear();

        var actualPhraseDictionaries = _dbContext.PhraseDictionaries
            .OrderBy(pd => pd.Name)
            .Include(pd => pd.Phrases);
        var actualPhraseDictionary = Assert.Single(actualPhraseDictionaries);

        Assert.IsType<int>(actualPhraseDictionary.PhraseDictionaryId);
        Assert.Equal(new string('a', count: 255), actualPhraseDictionary.Name);
        Assert.Equal(PhraseDictionaryFormat.DleTxt, actualPhraseDictionary.Format);
        Assert.Equal("test.txt", actualPhraseDictionary.FilePath);
        Assert.Null(actualPhraseDictionary.Description);
        Assert.Empty(actualPhraseDictionary.Phrases); 
    }
    
    [Fact]
    public async Task AddPhraseDictionaryAsync_WithInvalidPhraseDictionary_ThrowsDbUpdateException()
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = new string('a', count: 256),
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test.txt"
        };
        
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _dbContext.Database.BeginTransactionAsync();
            await _service.AddPhraseDictionaryAsync(phraseDictionary);
            _dbContext.ChangeTracker.Clear();
        });
    }
    
    [Fact]
    public async Task GetPhraseDictionariesAsync_WithNoPhraseDictionariesCreated_ReturnsEmptyEnumerable()
    {
        var actualPhraseDictionaries = await _service.GetPhraseDictionariesAsync();
        
        Assert.Empty(actualPhraseDictionaries);
    }
    
    [Fact]
    public async Task GetPhraseDictionariesAsync_WithOnePhraseDictionaryAdded_ReturnsOnePhraseDictionary()
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = "test",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test.txt"
        };
        await _dbContext.Database.BeginTransactionAsync();
        await _service.AddPhraseDictionaryAsync(phraseDictionary);
        _dbContext.ChangeTracker.Clear();
        
        var actualPhraseDictionaries = await _service.GetPhraseDictionariesAsync();

        Assert.Single(actualPhraseDictionaries);
    }
    
    [Fact]
    public async Task GetPhraseDictionariesAsync_WithTwoPhraseDictionariesAdded_ReturnsTwoPhraseDictionaries()
    {
        var phraseDictionary1 = new PhraseDictionary
        {
            Name = "test1",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test1.txt"
        };
        var phraseDictionary2 = new PhraseDictionary {
            Name = "test2",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test2.txt"
        };
        await _dbContext.Database.BeginTransactionAsync();
        await _service.AddPhraseDictionaryAsync(phraseDictionary1);
        await _service.AddPhraseDictionaryAsync(phraseDictionary2);
        _dbContext.ChangeTracker.Clear();
        
        var actualPhraseDictionaries = await _service.GetPhraseDictionariesAsync();

        Assert.Equal(2, actualPhraseDictionaries.Count());
    }
    
    [Fact]
    public async Task UpdatePhraseDictionaryAsync_WithValidPhraseDictionary_UpdatesTheDictionaryInDatabase()
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = "test",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = "test.txt",
            Phrases =
            {
                new Phrase
                {
                    Name = "test",
                    RegExPattern = "test",
                    BaseWord = "test",
                    Definitions = []
                }
            }
        };
        await _dbContext.Database.BeginTransactionAsync();
        await _service.AddPhraseDictionaryAsync(phraseDictionary);

        phraseDictionary.Name = "test2";
        phraseDictionary.Description = "test description";
        phraseDictionary.FilePath = "test2.txt";
        await _service.UpdatePhraseDictionaryAsync(phraseDictionary);
        _dbContext.ChangeTracker.Clear();
        var actualPhraseDictionaries = _dbContext.PhraseDictionaries
            .OrderBy(pd => pd.Name)
            .Include(pd => pd.Phrases);
        var actualPhraseDictionary = Assert.Single(actualPhraseDictionaries);

        Assert.IsType<int>(actualPhraseDictionary.PhraseDictionaryId);
        Assert.Equal("test2", actualPhraseDictionary.Name);
        Assert.Equal(PhraseDictionaryFormat.DleTxt, actualPhraseDictionary.Format);
        Assert.Equal("test2.txt", actualPhraseDictionary.FilePath);
        Assert.Equal("test description", actualPhraseDictionary.Description);
        Assert.Single(actualPhraseDictionary.Phrases);
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
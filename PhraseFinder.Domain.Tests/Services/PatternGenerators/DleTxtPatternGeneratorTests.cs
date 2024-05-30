using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.PatternGenerators;

namespace PhraseFinder.Domain.Tests.Services.PatternGenerators;

public class DleTxtPatternGeneratorTests
{
	private readonly IPatternGenerator _patternGenerator = 
		PatternGeneratorFactory.CreateGenerator(PhraseDictionaryFormat.DleTxt);

	[Fact]
	public void GeneratePatterns_ForPhraseWithOnePattern_ReturnsOnePhrase()
	{
		var phrase = new Phrase
		{
			PhraseId = 3,
			BaseWord = "cuenta",
			Value = "a buena cuenta",
			Categories = "loc. adv."
		};

		var result = _patternGenerator.GeneratePatterns(phrase).ToArray();

		Assert.Single(result);
		Assert.Equal(new PhrasePattern
        {
			Variant = phrase.Value,
			Pattern = phrase.Value,
			BaseWord = phrase.BaseWord,
			PhraseId = phrase.PhraseId
        }, result.Single());
	}

	[Fact]
	public void GeneratePatterns_ForPhraseWithMultiplePatterns_ReturnsMultiplePhrases()
	{
		var phrase = new Phrase
		{
			PhraseId = 23,
			BaseWord = "aviado, da",
			Value = "estar, o ir, aviado, da",
            Categories = "loc. adv."
        };
        var expectedPattern = new PhrasePattern
        {
			Variant = "estar aviado",
			Pattern = "estar aviado",
            BaseWord = "aviado, da",
            PhraseId = phrase.PhraseId
        };
		HashSet<PhrasePattern> expectedResults =
		[
			expectedPattern,
            expectedPattern with
			{
				Variant = "estar aviada",
				Pattern = "estar aviada"
			},
            expectedPattern with
			{
				Variant = "ir aviado",
				Pattern = "ir aviado"
			},
            expectedPattern with
			{
				Variant = "ir aviada",
				Pattern = "ir aviada"
			}
		];

		var results = _patternGenerator.GeneratePatterns(phrase).ToHashSet();

		Assert.Equal(expectedResults, results);
	}
}
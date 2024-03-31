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
			BaseWord = "cuenta",
			Value = "a buena cuenta"
		};

		var result = _patternGenerator.GeneratePatterns(phrase).ToArray();

		Assert.Single(result);
		Assert.Equal(phrase with {
			Variant = "a buena cuenta",
			Pattern = "a buena cuenta"
		}, result.Single());
	}

	[Fact]
	public void GeneratePatterns_ForPhraseWithMultiplePatterns_ReturnsMultiplePhrases()
	{
		var phrase = new Phrase
		{
			BaseWord = "aviado, da",
			Value = "estar, o ir, aviado, da"
		};
		HashSet<Phrase> expectedResults =
		[
			phrase with
			{
				Variant = "estar aviado",
				Pattern = "estar aviado"
			},
			phrase with
			{
				Variant = "estar aviada",
				Pattern = "estar aviada"
			},
			phrase with
			{
				Variant = "ir aviado",
				Pattern = "ir aviado"
			},
			phrase with
			{
				Variant = "ir aviada",
				Pattern = "ir aviada"
			}
		];

		var results = _patternGenerator.GeneratePatterns(phrase).ToHashSet();

		Assert.Equal(expectedResults, results);
	}
}
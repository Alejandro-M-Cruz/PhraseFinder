using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;

public class MultipleVariantPhraseSplitter : IPhraseSplitter
{
	private static readonly Regex MultipleVariantRegex = new(
		"^(.+?)(,(?: o .+?,){1,} o .+?)(, .+?|)$",
		RegexOptions.Compiled);

	private static readonly TwoVariantPhraseSplitter Splitter = new();

	public string[] SplitPhrase(string phrase)
	{
		var match = MultipleVariantRegex.Match(phrase);

		if (!match.Success)
		{
			return [phrase];
		}

		var firstPart = match.Groups[1].Value;
		var options = match.Groups[2].Value.Split(", o ").Skip(1);
		var lastPart = match.Groups[3].Value;
		List<string> phrases = [firstPart + (lastPart != "" ? lastPart[1..] : "")];

		foreach (var option in options)
		{
			var splits = Splitter.SplitPhrase(firstPart + ", o " + option + lastPart);
			if (splits.Length != 2)
			{
				continue;
			}
			phrases.Add(splits[1]);
		}

		return phrases.ToArray();
	}

}
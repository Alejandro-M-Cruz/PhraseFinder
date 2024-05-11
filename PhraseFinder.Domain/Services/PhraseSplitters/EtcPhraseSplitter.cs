using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;

public class EtcPhraseSplitter : IPhraseSplitter
{
    private static readonly Regex PhraseWithEtcRegex = new(
		@"^(.*?)(?:, )?((?:\w+ )*\w+), etc\.(?:,( .+))?$",
        RegexOptions.Compiled);


    public string[] SplitPhrase(string phrase)
    {
        var match = PhraseWithEtcRegex.Match(phrase);

	    if (!match.Success)
	    {
		    return [phrase];
	    }

		var optionsButLast = match.Groups[1].Value;
		var lastOption = match.Groups[2].Value;
		var lastPart = match.Groups[3].Value;

		if (string.IsNullOrWhiteSpace(optionsButLast))
		{
			return [lastOption + lastPart];
		}

		var options = optionsButLast.Split(", ").Append(lastOption).ToArray();
		FilterOptions(ref options);
		var firstPartWithFirstOption = options[0].Split();
		var wordsToTake = Math.Min(firstPartWithFirstOption.Length, options.Last().Split().Length);
		var firstPart = string.Join(' ', firstPartWithFirstOption[..^wordsToTake]);
		var firstOptionWords = firstPartWithFirstOption.TakeLast(wordsToTake).ToArray();
		options[0] = string.Join(' ', firstOptionWords);

		return options
			.Select(option => 
				(string.IsNullOrWhiteSpace(firstPart) ? "" : firstPart + " ") + option + lastPart)
			.ToArray();
    }

	private static void FilterOptions(ref string[] options)
	{
		var prefixed = options.Count(option => option.StartsWith("o "));
		if (prefixed == 0)
		{
			return;
		} 
		if (prefixed < options.Length - 1)
		{
			var lastNonPrefixed = Array.FindLastIndex(options, option => !option.StartsWith("o "));
			var firstPartWithFirstOption = string.Join(", ", options.Take(lastNonPrefixed + 1));
			options = options[lastNonPrefixed..];
			options[0] = firstPartWithFirstOption;
		}
		for (var i = 1; i < options.Length; i++)
		{
			options[i] = options[i][2..];
		}
	}
}
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

		var firstGroup = match.Groups[1];
		var secondGroup = match.Groups[2];
		var lastOption = secondGroup.Value;
		var lastPart = match.Groups[3].Value;

		if (string.IsNullOrWhiteSpace(firstGroup.Value))
		{
			return [lastOption + lastPart];
		}

		var options = firstGroup.Value.Split(", ").Append(lastOption).ToArray();
		var firstPartWithFirstOption = options[0].Split();
		var wordsToTake = Math.Min(firstPartWithFirstOption.Length, lastOption.Split().Length);
		var firstPart = string.Join(' ', firstPartWithFirstOption[..^wordsToTake]);
		var firstOptionWords = firstPartWithFirstOption.TakeLast(wordsToTake).ToArray();
		options[0] = string.Join(' ', firstOptionWords);

		if (options[1..].All(option => option.StartsWith("o ")))
		{
			RemoveOptionPrefix(ref options);
		}

		return options
			.Select(option => 
				(string.IsNullOrWhiteSpace(firstPart) ? "" : firstPart + " ") + option + lastPart)
			.ToArray();
    }

    private void RemoveOptionPrefix(ref string[] options)
    {
	    for (var i = 1; i < options.Length; i++)
	    {
			options[i] = options[i][2..];
		}
    }
}
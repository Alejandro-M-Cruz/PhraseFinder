using System.Diagnostics;
using System.Text.RegularExpressions;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.PerformanceTests;

//PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(  
//    new Regex(
//        @"\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as)", 
//        RegexOptions.Compiled),
//    @"D:\Proyectos\dotNet\TFT\expresiones-con-genero.txt");

//PerformanceTestUtils.RegexMatchGroupsExample();

//PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
//    new Regex(@"o, \w{3,}a\b"),
//    @"D:\Proyectos\dotNet\TFT\femeninos.txt");

//var regex = new Regex(
//    @"\b\w*(\w)(o(s|)),\s\w?\1(a\3)\b|\b\w*([^aeiouAEIOUlL\W])(e(s|)),\s\w?\5(a\7)\b|\b\w+(o([^aeiouAEIOUsS\W])),\s(\10(a))\b");

//PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
//       regex, @"D:\Proyectos\dotNet\TFT\expresiones-con-genero.txt");



var phraseDictionaryService = new PhraseDictionaryService(
	PerformanceTestUtils.DbContext);

var stopwatch = Stopwatch.StartNew();

await phraseDictionaryService.AddPhraseDictionaryFromFileAsync(
	new PhraseDictionary
	{
		Name = "Diccionario1",
		Format = PhraseDictionaryFormat.DleTxt,
		FilePath = "D:\\Proyectos\\dotNet\\TFT\\DLE.txt",
		AddedAt = DateTime.Now
	});

stopwatch.Stop();

Console.WriteLine($"Dictionary added in: {stopwatch.ElapsedMilliseconds} ms");



PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
    new Regex(@"(.*, o.*){2,}"),
    @"D:\Proyectos\dotNet\TFT\multiple_variants.txt");

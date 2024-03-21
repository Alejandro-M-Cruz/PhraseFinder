using System.Text.RegularExpressions;
using PhraseFinder.Domain.PerformanceTests;

//PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(  
//    new Regex(
//        @"\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as)", 
//        RegexOptions.Compiled),
//    @"D:\Proyectos\dotNet\TFT\expresiones-con-genero.txt");

PerformanceTestUtils.RegexMatchGroupsExample();

PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
    new Regex(@"o, \w{3,}a\b"),
    @"D:\Proyectos\dotNet\TFT\femeninos.txt");

var regex = new Regex(
    @"\b\w*(\w)(o(s|)),\s\w?\1(a\3)\b|\b\w*([^aeiouAEIOUlL\W])(e(s|)),\s\w?\5(a\7)\b|\b\w+(o([^aeiouAEIOUsS\W])),\s(\10(a))\b");

PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
       regex, @"D:\Proyectos\dotNet\TFT\expresiones-con-genero.txt");
using System.Text.RegularExpressions;
using PhraseFinder.Domain.PerformanceTests;

PerformanceTestUtils.WritePhrasesThatMatchRegexToTxtFile(
    new Regex(
        @"(\b(\w+(o|o[a-rt-zA-RT-Z]|e)),\s\b(\w{1,2}a)|\b(\w+(os|es)),\s\b(\w{1,2}as))", 
        RegexOptions.Compiled),
    @"D:\Proyectos\dotNet\TFT\expresiones-con-genero.txt");

PerformanceTestUtils.RegexMatchGroupsExample();

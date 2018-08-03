using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.BlacklistImport
{
    using System.Text.RegularExpressions;

    public class TextHandler
    {
        public string ChangeToLowercase(string input)
        {
            return input.ToLower();
        }

        public string RemoveTextBetweenBracketsAndBrackets(string input)
        {
            // *\([^)]*\
            // \(|\[.*?\]|\)
            // \(.*?\)
            // \[.*?\]
            // \(.*?\)|\[.*?\]
            var regex = @"\(.*?\)|\[.*?\]";
            var output = Regex.Replace(input, regex, " ");
            return output;
        }

        public string ReplaceBracketsNumbersMinusDotsWithSpace(string input)
        {
            var pattern = new Regex("[\\[\\]\\(\\)-0123456789\\.]");
            return pattern.Replace(input, " ");
        }

        public string[] SplitAtSpaces(string input)
        {
            return input.Split(' ');
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.BlacklistImport
{
    using System.Collections;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class TextHandler
    {
        public string ToLower(string input)
        {
            return input.ToLower();
        }

        public string RemoveTextBetweenBracketsAndBrackets(string input)
        {
            var regex = @"\(.*?\)|\[.*?\]";
            var output = Regex.Replace(input, regex, " ");
            return output;
        }

        public string ReplaceSpecialCharsWithSpace(string input)
        {
            var pattern = new Regex("[-0123456789\\.,'\"/]");
            return pattern.Replace(input, " ");
        }

        public string[] SplitAtSpaces(string input)
        {
            var splitted = input.Split(' ');
            var removedEmptyStrings = splitted.Where(val => !string.IsNullOrEmpty(val)).ToArray();
            return removedEmptyStrings;
        }

        public bool IsTooShort(string input)
        {
            return input.Length <= 2;
        }

        public string[] RemoveTooShortWords(string[] input)
        {
            var result = input.Where(val => !this.IsTooShort(val)).ToArray();
            return result;
        }

    }
}

﻿using System;
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
            var pattern = new Regex("[-0123456789\\.,'’‘`´י\"/]");
            return pattern.Replace(input, " ");
        }

        public string[] SplitAtLineBreaks(string input)
        {
            string[] lines = input.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries
            );
            return lines;
        }

        public string PutTogetherAtSpaces(string input)
        {
            return input.Replace(" ", "");
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

        public string RemoveTooShortTextElementsAtSpace(string input)
        {
            var lines = input.Split(' ');
            var withoutShortWords = RemoveTooShortWords(lines);
            return string.Join(" ", withoutShortWords);
        }

    }
}

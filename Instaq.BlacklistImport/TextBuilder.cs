using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.BlacklistImport
{
    public class TextBuilder
    {
        private readonly TextHandler textHandler;

        public TextBuilder()
        {
            this.textHandler = new TextHandler();
        }

        public IEnumerable<string> GetCleanList(IEnumerable<string> entries)
        {
            var result = new List<string>();
            foreach (var entry in entries)
            {
                var cleanText = ProcessText(entry);
                foreach (var text in cleanText)
                {
                    if (!result.Contains(text))
                    {
                        result.Add(text);
                    }
                }
            }
            return result;
        }

        public string[] ProcessText(string input)
        {
            var toLower = this.textHandler.ToLower(input);
            var noBrackets = this.textHandler.RemoveTextBetweenBracketsAndBrackets(toLower);
            var noSpecialChars = this.textHandler.ReplaceSpecialCharsWithSpace(noBrackets);
            // Remove too shorta
            var noShortTextElements = this.textHandler.RemoveTooShortTextElementsAtSpace(noSpecialChars);

            var noSpaces = this.textHandler.PutTogetherAtSpaces(noShortTextElements);
            var splitted = this.textHandler.SplitAtLineBreaks(noSpaces);
            var noShortTexts = this.textHandler.RemoveTooShortWords(splitted);
            return noShortTexts;
        }

    }
}

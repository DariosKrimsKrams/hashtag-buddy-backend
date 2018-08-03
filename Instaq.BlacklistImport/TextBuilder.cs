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

        public string[] ProcessText(string input)
        {
            var toLower = this.textHandler.ToLower(input);
            var noBrackets = this.textHandler.RemoveTextBetweenBracketsAndBrackets(toLower);
            var noSpecialChars = this.textHandler.ReplaceNumbersMinusDotsCommasWithSpace(noBrackets);
            var splitted = this.textHandler.SplitAtSpaces(noSpecialChars);
            var noShortTexts = this.textHandler.RemoveTooShortWords(splitted);
            return noShortTexts;
        }

    }
}

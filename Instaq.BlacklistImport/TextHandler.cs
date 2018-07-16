using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.BlacklistImport
{
    using System.Text.RegularExpressions;

    class TextHandler
    {

        private string RemoveTextInsideBrackets(string text)
        {
            // *\([^)]*\
            text = Regex.Replace(text, @" ?\(.*?\)", string.Empty);
            return text;
        }
    }
}

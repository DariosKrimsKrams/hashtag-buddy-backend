namespace Instaq.BlacklistImport.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class TextHandler_WhenInsertText
    {
        private readonly TextHandler textHandler;

        public TextHandler_WhenInsertText()
        {
            this.textHandler = new TextHandler();
        }

        [TestCase("Istanbul", "istanbul")]
        [TestCase("LONDON", "london")]
        [TestCase("kinshasA", "kinshasa")]
        [TestCase("johannesBurg", "johannesburg")]
        public void ThenAnyChar_ShouldBeLowercase(string input, string expected)
        {
            var result = this.textHandler.ToLower(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("Istanbul (Asiatischer Teil)", "Istanbul  ")]
        [TestCase("Zhōngguó(Zhōnghuá Rénmín Gònghéguó) 中国(中华人民共和国)", "Zhōngguó  中国 ")]
        [TestCase("Anchorage[1337]", "Anchorage ")]
        [TestCase("[19]Anchorage", " Anchorage")]
        [TestCase("Anchorage[q] Test", "Anchorage  Test")]
        [TestCase("test (a) test2 [b] test3", "test   test2   test3")]
        [TestCase("test (b", "test (b")]
        [TestCase("test]b", "test]b")]
        public void ThenTextBetweenBracketsAndBrackets_ShouldBeRemoved(string input, string expected)
        {
            var result = this.textHandler.RemoveTextBetweenBracketsAndBrackets(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("Washington D.C.", "Washington D C ")]
        [TestCase("Test 123 456 7890", "Test             ")]
        [TestCase("Bla-Blubb", "Bla Blubb")]
        [TestCase("Bla, Blubb", "Bla  Blubb")]
        [TestCase("x'y", "x y")]
        [TestCase("x\"y", "x y")]
        [TestCase("x/y", "x y")]
        [TestCase("'’‘`´יmoon day'", "      moon day ")]
        public void ThenSpecialChars_ShouldBeReplacedWithSpace(string input, string expected)
        {
            var result = this.textHandler.ReplaceSpecialCharsWithSpace(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("Washington D C ", "Washington")]
        [TestCase("Washington DC", "Washington")]
        [TestCase("Washington ABC", "Washington ABC")]
        [TestCase("   abc    defg  ", "abc defg")]
        public void ThenTooShortTextElements_ShouldBeRemoved(string input, string expected)
        {
            var result = this.textHandler.RemoveTooShortTextElementsAtSpace(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("a\r\nb", new string[] { "a", "b" })]
        [TestCase("a\nb", new string[] { "a", "b" })]
        [TestCase("a\rb", new string[] { "a", "b" })]
        [TestCase(" a  \n   bc ", new string[] { " a  ", "   bc " })]
        [TestCase("\r\na\r\n\r\nb\r\nc\r\n\r\n", new string[] { "a", "b", "c" })]
        public void ThenTextWithLineBreaks_ShouldSplitAtLineBreak(string input, string[] expected)
        {
            var result = this.textHandler.SplitAtLineBreaks(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("ab c", "abc")]
        [TestCase(" a  b    c   ", "abc")]
        public void ThenTextWithSpaces_ShouldPutTogetherAtSpaces(string input, string expected)
        {
            var result = this.textHandler.PutTogetherAtSpaces(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("a")]
        [TestCase("ab")]
        public void ThenTooShortWords_ShouldBeIdentify(string input)
        {
            var result = this.textHandler.IsTooShort(input);
            Assert.True(result);
        }

        [TestCase("abc")]
        public void ThenTooShortWords_ShouldNotBeIdentify(string input)
        {
            var result = this.textHandler.IsTooShort(input);
            Assert.False(result);
        }

        [TestCase(new string[] { "a", "ab", "abc", "→", "test", "x"}, new string[] { "abc", "test" })]
        public void ThenTooShortWords_ShouldBeRemoved(string[] input, string[] expected)
        {
            var result = this.textHandler.RemoveTooShortWords(input);
            Assert.AreEqual(result, expected);
        }
    }
}

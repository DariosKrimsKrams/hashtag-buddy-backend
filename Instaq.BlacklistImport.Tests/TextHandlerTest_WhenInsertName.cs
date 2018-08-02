namespace Instaq.BlacklistImport.Tests
{
    using Xunit;

    public class TextHandlerTest_WhenInsertName
    {
        private TextHandler textHandler;

        public TextHandlerTest_WhenInsertName()
        {
            textHandler = new TextHandler();
        }

        [Theory]
        [InlineData("Istanbul", "istanbul")]
        [InlineData("LONDON", "london")]
        [InlineData("kinshasA", "kinshasa")]
        [InlineData("johannesBurg", "johannesburg")]
        public void ThenAnyChar_ShouldBeLowercase(string input, string expected)
        {
            var result = this.textHandler.ChangeToLowercase(input);
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("Istanbul (Asiatischer Teil)", "")]
        [InlineData("Zhōngguó(Zhōnghuá Rénmín Gònghéguó) 中国(中华人民共和国)", "Zhōngguó() 中国()")]
        [InlineData("Anchorage[1337]", "Anchorage[]")]
        [InlineData("[19]Anchorage", "[]Anchorage")]
        [InlineData("Anchorage[q] Test", "Anchorage[] Test")]
        [InlineData("test (a) test2 [b] test3", "test () test2 [] test3")]
        [InlineData("test (b", "test (b")]
        [InlineData("test]b", "test]b")]
        public void ThenCharsBetweenBrackets_ShouldBeRemoved(string input, string expected)
        {
            var result = this.textHandler.RemoveTextInsideBrackets(input);
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("][a[]b()c", "a  b  c  ")]
        [InlineData("Washington D.C.", "Washington D C ")]
        [InlineData("Test 123 456 7890", "Test             ")]
        public void ThenBracketsNumbersAndDots_ShouldBeReplacedWithSpace(string input, string expected)
        {
            var result = this.textHandler.ReplaceBracketsNumbersAndDotsWithSpace(input);
            Assert.Equal(result, expected);
        }

        //[Theory]
        //[InlineData("test1 test2", new string[] { "test1", "test2" })]
        //[InlineData(" test1  a   ", new string[] { "test1", "a" })]
        //public void ThenWordShouldSplittedAtSpaces(string input, string expected)
        //{

        //}

        [Theory]
        [InlineData("", "")]
        public void ThenWordShouldBeTrimed(string input, string expected)
        {

        }

        [Theory]
        [InlineData("", "")]
        public void ThenTooShouldWordsShouldBeRemoved(string input, string expected)
        {

        }

        /*
         * Washington D.C.
         * Nukuʻalofa
         * Ho-Chi-Minh-Stadt
         * Istanbul (Asiatischer Teil)
         * China, Republic of → Taiwan
         *
         *
         *
         */
    }
}

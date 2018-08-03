using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.BlacklistImport.Tests
{
    using Xunit;

    public class TextBuilder_WhenProcessText
    {
        private readonly TextBuilder textBuilder;
        public TextBuilder_WhenProcessText()
        {
            this.textBuilder = new TextBuilder();

        }

        [Fact]
        public void ThenGetCleanList_ShouldNotContainDuplicates()
        {
            var input = new List<string> { "Abc (xx) def.", "GHI abC [q]" };
            var expected = new List<string> { "abc", "def", "ghi" };
            var result = this.textBuilder.GetCleanList(input);
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("Washington D.C.", new string[] { "washington" })]
        [InlineData("Ho-Chi-Minh-Stadt", new string[] { "chi", "minh", "stadt" })]
        [InlineData("Istanbul (Asiatischer Teil)", new string[] { "istanbul" })]
        [InlineData("Virginia Beach[16]", new string[] { "virginia", "beach" })]
        [InlineData("China, Republic of → Taiwan", new string[] { "china", "republic", "taiwan" })]
        [InlineData(" Zhō'ab c(Zhōná mín)  中国(中国) abc [q]", new string[] { "zhō'ab", "abc" })]
        public void ThenAnyChar_ShouldBeLowercase(string input, string[] expected)
        {
            var result = this.textBuilder.ProcessText(input);
            Assert.Equal(result, expected);
        }
    }
}

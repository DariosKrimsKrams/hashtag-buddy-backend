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
            var input = new List<string> { "Abc (xx) def\r\nHey.", "io u GHI /abC [q]\nhey", "a-b-0123c" };
            var expected = new List<string> { "abcdef", "hey", "ghiabc" };
            var result = this.textBuilder.GetCleanList(input);
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("Washington D.C.", new string[] { "washington" })]
        [InlineData("Ho-Chi-Minh-Stadt\rHochi-Minh", new string[] { "chiminhstadt", "hochiminh" })]
        [InlineData("Istanbul (Asiatischer Teil)", new string[] { "istanbul" })]
        [InlineData("Virginia Beach[16]", new string[] { "virginiabeach" })]
        [InlineData("China, Republic of → Taiwan", new string[] { "chinarepublictaiwan" })]
        [InlineData(" Zhō'ab c(Zhōná mín)  中国(中国) xyz [q]", new string[] { "zhōxyz" })]
        public void ThenAnyChar_ShouldBeLowercase(string input, string[] expected)
        {
            var result = this.textBuilder.ProcessText(input);
            Assert.Equal(result, expected);
        }
    }
}

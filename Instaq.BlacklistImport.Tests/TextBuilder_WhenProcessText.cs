namespace Instaq.BlacklistImport.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TextBuilder_WhenProcessText
    {
        private readonly TextBuilder textBuilder;
        public TextBuilder_WhenProcessText()
        {
            this.textBuilder = new TextBuilder();

        }

        [Test]
        public void ThenGetCleanList_ShouldNotContainDuplicates()
        {
            var input = new List<string> { "Abc (xx) def\r\nHey.", "io u GHI /abC [q]\nhey", "a-b-0123c" };
            var expected = new List<string> { "abcdef", "hey", "ghiabc" };
            var result = this.textBuilder.GetCleanList(input);
            Assert.AreEqual(result, expected);
        }

        [TestCase("Washington D.C.", new string[] { "washington" })]
        [TestCase("Ho-Chi-Minh-Stadt\rHochi-Minh", new string[] { "chiminhstadt", "hochiminh" })]
        [TestCase("Istanbul (Asiatischer Teil)", new string[] { "istanbul" })]
        [TestCase("Virginia Beach[16]", new string[] { "virginiabeach" })]
        [TestCase("China, Republic of → Taiwan", new string[] { "chinarepublictaiwan" })]
        [TestCase(" Zhō'ab c(Zhōná mín)  中国(中国) xyz [q]", new string[] { "zhōxyz" })]
        public void ThenAnyChar_ShouldBeLowercase(string input, string[] expected)
        {
            var result = this.textBuilder.ProcessText(input);
            Assert.AreEqual(result, expected);
        }
    }
}

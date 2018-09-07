namespace Instaq.Crawler.Tests.Logic
{
    using System.Collections.Generic;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Crawler;
    using AutoTagger.Crawler.V4.Requests;
    using NSubstitute;
    using NUnit.Framework;

    class ImagePageLogic_WhenParsingHashtag
    {
        private ImagePageLogic logic;
        private ICrawlerSettings settings;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            var requestHandler = Substitute.For<IRequestHandler>();
            this.logic = new ImagePageLogic(this.settings, requestHandler);
        }

        [Test]
        public void ThenTextWithOneHashtagSeparatedBySpaces_ShouldBeReturnThisHashtag()
        {
            var text = "text #hashtag1 test";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag1" };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenTextWithMultipleHashtagSeparatedBySpaces_ShouldBeReturnThisHashtags()
        {
            var text = "text #hashtag1 test2 #hashtag2 test";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag1", "hashtag2" };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenTextWithMultipleHashtagNotSeparated_ShouldBeReturnThisHashtags()
        {
            var text = "#hashtag0 text #hashtag1 test2 #hashtag2#hashtag3 test#hashtag4";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag0", "hashtag1", "hashtag2", "hashtag3", "hashtag4" };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenHashtagsSeparatedBySpecialChars_ShouldMatchExpectedResult()
        {
            var text = "#hashtag0?test#hashtag1.#hashtag2, bla#hashtag3-#hashtag4!";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag0", "hashtag1", "hashtag2", "hashtag3", "hashtag4" };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenHashtagsSeparatedByLineBreaks_ShouldMatchExpectedResult()
        {
            var text = "#hashtag0#hashtag1\n#hashtag2\r\n bla#hashtag3\r#hashtag4";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag0", "hashtag1", "hashtag2", "hashtag3", "hashtag4" };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenHashtagsWithUppercase_ShouldBeLowercase()
        {
            var text = "#Hashtag1 #HASHTAG2 #haShTaG3";

            var hashtags = this.logic.ParseHashtags(text);

            var expected = new List<string> { "hashtag1", "hashtag2", "hashtag3", };
            Assert.AreEqual(hashtags, expected);
        }

        [Test]
        public void ThenNullHashtag_ShouldBeReturnEmptyList()
        {
            string text = null;

            var hashtags = this.logic.ParseHashtags(text);

            Assert.IsEmpty(hashtags);
        }
    }
}

namespace Instaq.Crawler.Tests.Handler
{
    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Crawler.V4.Crawler;
    using Instaq.Crawler.V4.Requests;
    using NSubstitute;
    using NUnit.Framework;

    class ImagePageLogic_WhenCheckingHashtagIsAllowed
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
        public void ThenValidHashtag_ShouldBeTrue()
        {
            var hashtag = "test";

            var status = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenHashtagWithNull_ShouldBeFalse()
        {
            string hashtag = null;

            var status = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenHashtagWithWhitespace_ShouldBeFalse()
        {
            var hashtag = "   ";

            var status = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenEmptyHashtag_ShouldBeFalse()
        {
            var hashtag = "";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenTooShortHashtag_ShouldBeFalse()
        {
            this.settings.MinHashtagLength = 5;
            var hashtag = "dari";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenLongEnoughHashtag_ShouldBeTrue()
        {
            this.settings.MinHashtagLength = 5;
            var hashtag = "dario";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenTooLongHashtag_ShouldBeFalse()
        {
            this.settings.MaxHashtagLength = 10;
            var hashtag = "darioiscool";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenShortEnoughHashtag_ShouldBeTrue()
        {
            this.settings.MaxHashtagLength = 10;
            var hashtag = "darioiscoo";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenAShortHashtagAndMaxHashtagLengthOf0_ShouldBeTrue()
        {
            this.settings.MaxHashtagLength = 0;
            var hashtag = "a";

            var status = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenHashtagWithOnlyDigits_ShouldBeFalse()
        {
            var hashtag = "1234567890";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenHashtagWithDigitsAndAtLeastOneChar_ShouldBeTrue()
        {
            var hashtag = "123456a7890";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenHashtagWithSingleQuotes_ShouldBeFalse()
        {
            var hashtag = "bday'91";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenHashtagWithAnySpecialCharsExcludingSingleQuotes_ShouldBeTrue()
        {
            var hashtag = "!\"§$%&/()={[]}?\\`´*+~'#-_.:,;@€";

            var status  = this.logic.HashtagIsAllowed(hashtag);

            Assert.IsFalse(status);
        }
    }
}

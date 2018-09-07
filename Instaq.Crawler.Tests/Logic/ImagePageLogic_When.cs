namespace Instaq.Crawler.Tests.Handler
{
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Crawler;
    using NUnit.Framework;

    class ImagePageLogic_When
    {
        private ImagePageLogic logic;
        private ICrawlerSettings settings;
        //private IUser user;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            //this.logic  = new ImagePageLogic(this.settings);
            //this.user     = new User { FollowerCount = 1337 };
        }

        [Test]
        public void ThenParsIngHashTags_ShouldY()
        {
        }
        [Test]
        public void ThenHashtagIsAllowed_ShouldY()
        {
        }
    }
}

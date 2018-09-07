namespace Instaq.Crawler.Tests.Crawler
{
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.PageAnalyzer;
    using NUnit.Framework;

    class UserPageLogic_WhenCheckingEnoughFollower
    {
        private UserPageLogic logic;
        private ICrawlerSettings settings;
        private IUser user;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            this.logic = new UserPageLogic(settings);
            this.user = new User { FollowerCount = 1337 };
        }

        [Test]
        public void ThenUserHasEnough_ShouldReturnTrue()
        {
            this.settings.UserMinFollowerCount = 1337;

            var status = this.logic.HasUserEnoughFollower(this.user);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenUserHasNotEnough_ShouldReturnFalse()
        {
            this.settings.UserMinFollowerCount = 1338;

            var status = this.logic.HasUserEnoughFollower(this.user);

            Assert.IsFalse(status);
        }
    }
}

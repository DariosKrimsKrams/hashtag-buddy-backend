namespace Instaq.Crawler.Tests.Logic
{
    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Crawler.V4.PageAnalyzer;
    using NUnit.Framework;

    [TestFixture]
    class UserPageLogic_WhenCheckingEnoughFollower
    {
        private UserPageLogic logic;
        private ICrawlerSettings settings;
        private IUser user;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            this.logic    = new UserPageLogic(this.settings);
            this.user     = new User { FollowerCount = 1337 };
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

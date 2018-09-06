using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Crawler.Tests.Crawler
{
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4;
    using AutoTagger.Crawler.V4.Crawler;
    using AutoTagger.Crawler.V4.PageAnalyzer;
    using AutoTagger.Crawler.V4.Requests;

    using Newtonsoft.Json;

    using NSubstitute;

    using NUnit.Framework;

    class UserPageHandler_WhenCheckingEnoughFollower
    {
        private UserPageHandler handler;
        private ICrawlerSettings settings;
        private IUser user;

        [SetUp]
        public void Setup()
        {
            this.settings = new CrawlerSettings();
            this.handler = new UserPageHandler(settings);
            this.user = new User { FollowerCount = 1337 };
        }

        [Test]
        public void ThenUserHasEnough_ShouldReturnTrue()
        {
            this.settings.UserMinFollowerCount = 1337;
            var status = this.handler.HasUserEnoughFollower(this.user);
            Assert.IsTrue(status);
        }

        [Test]
        public void ThenUserHasNotEnough_ShouldReturnFalse()
        {
            this.settings.UserMinFollowerCount = 1338;
            var status = this.handler.HasUserEnoughFollower(this.user);
            Assert.IsFalse(status);
        }
    }
}

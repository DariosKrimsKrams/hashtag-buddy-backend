using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Crawler.Tests
{
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4;
    using AutoTagger.Crawler.V4.Queue;

    using NUnit.Framework;

    class CrawlerV4_WhenCrawling
    {
        private ICrawler Crawler;

        [SetUp]
        public void Setup()
        {
            this.Crawler = new CrawlerV4();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}

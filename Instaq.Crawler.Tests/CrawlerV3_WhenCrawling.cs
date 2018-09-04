using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Crawler.Tests
{
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V3;
    using AutoTagger.Crawler.V3.Queue;

    using NUnit.Framework;

    class CrawlerV3_WhenCrawling
    {
        private ICrawler Crawler;

        [SetUp]
        public void Setup()
        {
            this.Crawler = new CrawlerV3();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}

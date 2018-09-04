namespace Tests
{
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V3;

    using NUnit.Framework;

    class CrawlerTest
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

namespace AutoTagger.Test.Core
{
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V3;
    using AutoTagger.Crawler.V3.Crawler;
    using AutoTagger.Database.Storage.Mysql;
    using Xunit;
    using Xunit.Abstractions;

    public class CrawlerTests
    {
        private readonly ICrawlerStorage db;

        public CrawlerTests(ITestOutputHelper testConsole)
        {
            this.testConsole = testConsole;
            //this.db = new LiteCrawlerStorage("test.ldb");
            this.db = new MysqlCrawlerStorage();
        }

        private ITestOutputHelper testConsole { get; }

        [Fact]
        public void CrawlerTest()
        {
            var crawler = new CrawlerV3();
            var crawlerApp = new CrawlerApp(this.db, crawler);

            //crawler.DoCrawling(1, "gratidão");
            //crawler.DoCrawling(1);
            crawlerApp.DoCrawling(0);
        }

        [Fact]
        public void RandomHashtagsTest()
        {
            var crawler  = new RandomTagsCrawler();
            var hashtags = crawler.Parse().ToList();

            foreach (var hashtag in hashtags)
            {
                this.testConsole.WriteLine(hashtag);
            }

            Assert.True(hashtags.Count > 2, "not enough random hashtags");
        }

    }
}

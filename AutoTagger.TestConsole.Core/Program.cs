namespace AutoTagger.TestConsole.Core
{
    using System;
    using System.Linq;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.Standard.V1;
    using AutoTagger.Database.Storage.AutoTagger;
    using AutoTagger.Database.Storage.Mysql;
    using AutoTagger.ImageProcessor.Standard;
    using AutoTagger.ImageDownloader.Standard;

    internal class Program
    {

        private static void Main(string[] args)
        {
            Console.WriteLine("" + 
                             "1: Start Crawler (CrawlerEngine V1)\n" +
                             "2: Start Image Downloader\n" +
                             "3: Start ImageProcessor (GCP Vision)\n" +
                             "4: Crawl Mtags with HighScore\n" +
                             ""
                             );
            while(true)
            {
                var key = Console.ReadKey();
                Console.WriteLine("");
                switch (key.KeyChar)
                {
                    case '1':
                        Console.WriteLine("Start Crawler...");
                        StartCrawler();
                        break;

                    case '2':
                        Console.WriteLine("Download Images...");
                        StartImageDownloader();
                        break;

                    case '3':
                        Console.WriteLine("Start Image Processor...");
                        StartImageProcessor();
                        break;

                    case '4':
                        Console.WriteLine("Start CrawlMtagsWithHighScore...");
                        CrawlMtagsWithHighScore();
                        break;
                }
                Console.WriteLine("------------");
            }
            
        }

        private static void CrawlMtagsWithHighScore()
        {
            var uiDb = new MysqlUIStorage();
            var mtags = uiDb.GetMtagsWithHighScore();
            var mtagsArr = mtags.Select(m => m.Replace(" ", "").ToLower()).ToArray();

            var crawlerDb = new MysqlCrawlerStorage();
            var crawlerEngine = new CrawlerV1();
            crawlerEngine.OverrideCondition("MinPostsForHashtags", 10 * 1000);
            crawlerEngine.BuildTags(mtagsArr);
            crawlerEngine.DisableFurtherEnqueue();

            var crawler = new CrawlerApp(crawlerDb, crawlerEngine);
            crawler.OnImageSaved += image =>
            {
                Console.WriteLine(
                    "{ \"shortcode\":\"" + image.Shortcode + "\", \"from\":\"" + image.User + "\", \"tags\": ["
                  + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'")) + "], \"uploaded\":\""
                  + image.Uploaded + "\", " + "\"likes\":\"" + image.Likes + "\", \"follower\":\"" + image.Follower
                  + "\", \"comments\":\"" + image.Comments + "\", }");
            };
            crawler.DoCrawling(0);
        }

        private static void StartCrawler()
        {
            var db = new MysqlCrawlerStorage();
            var crawler = new CrawlerApp(db, new CrawlerV1());

            crawler.OnImageSaved += image =>
            {
                Console.WriteLine(
                    "{ \"shortcode\":\"" + image.Shortcode + "\", \"from\":\"" + image.User + "\", \"tags\": ["
                  + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'")) + "], \"uploaded\":\""
                  + image.Uploaded + "\", " + "\"likes\":\"" + image.Likes + "\", \"follower\":\"" + image.Follower
                  + "\", \"comments\":\"" + image.Comments + "\", }");
            };
            crawler.DoCrawling(0);
        }

        private static void StartImageDownloader()
        {
            var db = new MysqlImageProcessorStorage();
            var imageDownloader = new ImageDownloader(db);
            imageDownloader.Start();
        }

        private static void StartImageProcessor()
        {
            var db = new MysqlImageProcessorStorage();
            var tagger = new GCPVision();

            var imageProcessor = new ImageProcessorApp(db, tagger);
            ImageProcessorApp.OnLookingForTags += image =>
            {
                Console.WriteLine("Sending to GCP for " + image.Shortcode + "(" + image.Id + ")");
            };
            ImageProcessorApp.OnFoundTags += image =>
            {
                Console.WriteLine("Tags found for " + image.Shortcode + "(" + image.Id + ")");
            };
            ImageProcessorApp.OnDbInserted += image =>
            {
                Console.WriteLine("DB Insert for " + image.Shortcode +"("+image.Id+")");
            };
            ImageProcessorApp.OnDbSaved += () =>
            {
                Console.WriteLine("DB SAVED");
            };
            imageProcessor.Process();


        }
    }
}

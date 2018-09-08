namespace AutoTagger.TestConsole.Core
{
    using System;
    using System.Linq;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V4;
    using AutoTagger.Database.Storage.Mysql;
    using AutoTagger.ImageProcessor.Standard;
    using AutoTagger.ImageDownloader.Standard;

    using Instaq.BlacklistImport;
    using Instaq.TooGenericProcessor;

    internal class Program
    {

        private static void Main(string[] args)
        {
            Console.WriteLine("" + 
                             "1: Start Crawler (CrawlerEngine V4)\n" +
                             "2: Start Image Downloader\n" +
                             "3: Start ImageProcessor (GCP Vision)\n" +
                             "4: Crawl Mtags with HighScore\n" +
                             "5: Run Too Generic Processor\n" +
                             "6: Blacklist: Import csv\n" +
                             "7: Blacklist -> Set onBlacklist Flag\n" +
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

                    case '5':
                        Console.WriteLine("Run Too Generic Processor...");
                        RunTooGenericProcessor();
                        break;

                    case '6':
                        Console.WriteLine("Run BlacklistImport...");
                        RunBlacklistImport();
                        break;

                    case '7':
                        Console.WriteLine("Run Blacklist -> Set onBlacklist Flag...");
                        RunBlacklistSetItagFlags();
                        break;
                }
                Console.WriteLine("------------");
            }
            
        }

        private static void RunTooGenericProcessor()
        {
            var db = new MysqlTooGenericStorage();
            var processor = new TooGenericProcessor(db);
            processor.Start();
        }

        private static void CrawlMtagsWithHighScore()
        {
            var uiDb = new MysqlEvaluationStorage();
            var machineTags = uiDb.GetMtagsWithHighScore();
            var machineTagsArr = machineTags.Select(m => m.First().Replace(" ", "").ToLower()).ToArray();

            var crawlerDb = new MysqlCrawlerStorage();
            var requestHandler = new HttpRequestHandler();
            var crawlerEngine = new CrawlerV4(requestHandler);
            crawlerEngine.SetSetting("MinPostsForHashtags", 10 * 1000);
            crawlerEngine.InsertTags(machineTagsArr);
            //crawlerEngine.DisableFurtherEnqueue();

            var crawler = new CrawlerApp(crawlerDb, crawlerEngine);
            crawler.OnImageSaved += image =>
            {
                Console.WriteLine(
                    "{ \"shortcode\":\"" + image.Shortcode + "\", \"from\":\"" + image.User + "\", \"tags\": ["
                  + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'")) + "], \"uploaded\":\""
                  + image.Uploaded + "\", " + "\"likes\":\"" + image.Likes + "\", \"follower\":\"" + image.Follower
                  + "\", \"comments\":\"" + image.CommentCount + "\", }");
            };
            crawler.DoCrawling(0);
        }

        private static void StartCrawler()
        {
            var db = new MysqlCrawlerStorage();
            var requestHandler = new HttpRequestHandler();
            var crawler = new CrawlerApp(db, new CrawlerV4(requestHandler));

            crawler.OnImageSaved += image =>
            {
                Console.WriteLine(
                    "{ \"shortcode\":\"" + image.Shortcode + "\", \"from\":\"" + image.User + "\", \"tags\": ["
                  + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'")) + "], \"uploaded\":\""
                  + image.Uploaded + "\", " + "\"likes\":\"" + image.Likes + "\", \"follower\":\"" + image.Follower
                  + "\", \"comments\":\"" + image.CommentCount + "\", }");
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
            var tagger = new GcpVision();

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

        private static void RunBlacklistImport()
        {
            var db = new MysqlBlacklistStorage();
            var app = new BlacklistImportApp(db);
            var filename = @"C:\Users\dario\Documents\SourceTree\instaq\doc\days.csv";
            app.ReadCsv(filename);
            Console.WriteLine("Finished");
        }

        private static void RunBlacklistSetItagFlags()
        {
            var db = new MysqlBlacklistStorage();
            var app = new BlacklistImportApp(db);
            app.SetItagOnBlacklistFlags();
            Console.WriteLine("Finished");
        }
    }
}

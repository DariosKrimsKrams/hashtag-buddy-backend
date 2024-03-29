﻿namespace Instaq.TestConsole.Core
{
    using System;
    using System.Linq;
    using Instaq.Common;
    using Instaq.Crawler.V4;
    using Instaq.Database.Storage.Mysql;
    using Instaq.ImageDownloader;
    using Instaq.BlacklistImport;
    using Instaq.Crawler.V4.Helper;
    using Instaq.Database.Storage.Mysql.Generated;
    using Instaq.ImageProcessor.Standard;
    using Instaq.ImageProcessor.Standard.GcpVision;
    using Instaq.TooGenericProcessor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.PlatformAbstractions;

    internal class Program
    {

        private static InstaqContext context;

        private static void Main()
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var path = "SharedSettings.json";
            var pathEnv = $"SharedSettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(path, optional: true, reloadOnChange: true)
                .AddJsonFile(pathEnv, optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var dbConnection  = configuration.GetConnectionString("HashtagDatabase");
            dbConnection = dbConnection.Replace("[server]", Environment.GetEnvironmentVariable("instatagger_mysql_ip"));
            dbConnection = dbConnection.Replace("[user]", Environment.GetEnvironmentVariable("instatagger_mysql_user"));
            dbConnection = dbConnection.Replace("[pw]", Environment.GetEnvironmentVariable("instatagger_mysql_pw"));
            dbConnection = dbConnection.Replace("[db]", Environment.GetEnvironmentVariable("instatagger_mysql_db"));
            Console.WriteLine("Using DB: " + dbConnection);

            context = new InstaqContext(dbConnection);

            Console.WriteLine("" + 
                             "1: Start Crawler (CrawlerEngine V4)\n" +
                             "2: Start Image Downloader\n" +
                             "3: Start ImageProcessor (GCP Vision)\n" +
                             "4: Crawl Mtags with HighScore\n" +
                             "5: Calc RefCount (for all Humanoid Tags)\n" +
                             "6: Calc RefCount (for only Humanoid Tags with refCount=0)\n" +
                             "7: Blacklist: Import csv\n" +
                             "8: Blacklist -> Set onBlacklist Flag\n" +
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
                        Console.WriteLine("Run Too Generic Processor... (all Humanoid Tags)");
                        RunTooGenericProcessorAll();
                        break;

                    case '6':
                        Console.WriteLine("Run Too Generic Processor... (Humanoid Tags with refCount=0)");
                        RunTooGenericProcessorNew();
                        break;

                    case '7':
                        Console.WriteLine("Run BlacklistImport...");
                        RunBlacklistImport();
                        break;

                    case '8':
                        Console.WriteLine("Run Blacklist -> Set onBlacklist Flag...");
                        RunBlacklistSetItagFlags();
                        break;
                }
                Console.WriteLine("------------");
            }
            
        }

        private static void RunTooGenericProcessorAll()
        {
            var db = new MysqlTooGenericStorage(context);
            var processor = new TooGenericProcessor(db);
            processor.CalcHumanoidTagsRefCount();
        }

        private static void RunTooGenericProcessorNew()
        {
            var db        = new MysqlTooGenericStorage(context);
            var processor = new TooGenericProcessor(db);
            processor.CalcHumanoidTagsRefCount(true);
        }

        private static void CrawlMtagsWithHighScore()
        {
            var uiDb = new MysqlEvaluationStorage(context);
            var machineTags = uiDb.GetMtagsWithHighScore();
            var machineTagsArr = machineTags.Select(m => m.First().Replace(" ", "").ToLower()).ToArray();

            var crawlerDb = new MysqlCrawlerStorage(context);
            var requestHandler = new HttpRequestHandler();
            var settings = new CrawlerSettings
            {
                MinPostsForHashtags         = 10 * 1000,
                ExploreTagsMinHashtagCount  = 0,
                ExploreTagsMinLikes         = 100,
                ExploreTagsMinCommentsCount = 0,
                MaxHashtagLength            = 30,
                MinHashtagLength            = 5,
                UserMinFollowerCount        = 1000,
                UserMinHashTagCount         = 5,
                UserMinCommentsCount        = 10,
                UserMinLikes                = 300
            };
            var crawlerEngine = new CrawlerV4(requestHandler, settings);
            crawlerEngine.InsertTags(machineTagsArr);
            //crawlerEngine.DisableFurtherEnqueue();

            // ToDo

            //var crawler = new CrawlerBootstrap(crawlerDb, crawlerEngine);
            //crawler.OnImageSaved += image =>
            //{
            //    Console.WriteLine(
            //        "{ \"shortcode\":\"" + image.Shortcode + "\", \"from\":\"" + image.User + "\", \"tags\": ["
            //      + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'")) + "], \"uploaded\":\""
            //      + image.Uploaded + "\", " + "\"likes\":\"" + image.Likes + "\", \"follower\":\"" + image.User.FollowerCount
            //      + "\", \"comments\":\"" + image.CommentCount + "\", }");
            //};
            //crawler.DoCrawling();
        }

        private static void StartCrawler()
        {
            var db = new MysqlCrawlerStorage(context);
            new CrawlerBootstrap(db);
        }

        private static void StartImageDownloader()
        {
            var db = new MysqlImageProcessorStorage(context);
            var imageDownloader = new ImageDownloader(db);
            imageDownloader.Start();
        }

        private static void StartImageProcessor()
        {
            var db = new MysqlImageProcessorStorage(context);
            var tagger = new GcpVision();

            var imageProcessor = new ImageProcessorApp(db, tagger);
            ImageProcessorApp.OnLookingForTags += image =>
            {
                Console.WriteLine("Sending to GCP for " + image.Shortcode);
            };
            ImageProcessorApp.OnFoundTags += image =>
            {
                Console.WriteLine("Tags found for " + image.Shortcode);
            };
            ImageProcessorApp.OnDbInserted += image =>
            {
                Console.WriteLine("DB Insert for " + image.Shortcode);
            };
            ImageProcessorApp.OnDbSaved += () =>
            {
                Console.WriteLine("DB SAVED");
            };
            imageProcessor.Process();
        }

        private static void RunBlacklistImport()
        {
            var db = new MysqlBlacklistStorage(context);
            var app = new BlacklistImportApp(db);
            var folder = @"C:\Source\instaq-api\doc\blacklist";

            var filename = $@"{folder}\blacklist_cities.csv";
            app.ReadCsv(filename, "cities", "itags");
            Console.WriteLine("Finished Cities Import");

            filename = $@"{folder}\blacklist_days.csv";
            app.ReadCsv(filename, "days", "itags");
            Console.WriteLine("Finished Days Import");

            filename = $@"{folder}\blacklist_mtags.csv";
            app.ReadCsv(filename, "tooGeneric", "mtags");
            Console.WriteLine("Finished Too Generic Import");
        }

        private static void RunBlacklistSetItagFlags()
        {
            var db = new MysqlBlacklistStorage(context);
            var app = new BlacklistImportApp(db);
            app.SetBlacklistFlags();
            Console.WriteLine("Finished");
        }
    }
}

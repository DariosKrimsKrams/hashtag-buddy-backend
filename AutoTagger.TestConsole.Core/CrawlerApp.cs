using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.TestConsole
{
    using System.Linq;
    using System.Threading;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V4;

    using Google.Protobuf.WellKnownTypes;

    class CrawlerApp
    {
        private static ICrawlerStorage Db;
        private static readonly List<IImage> UpsertImages = new List<IImage>();
        private static readonly List<IHumanoidTag> UpsertHtags = new List<IHumanoidTag>();
        private static int savedHtagsCount;
        private static int savedImagesCount;
        private static DateTime startedDate;
        private static CrawlerV4 crawler;

        public CrawlerApp(ICrawlerStorage db)
        {
            Db = db;

            var requestHandler = new HttpRequestHandler();

            var settings = new CrawlerSettings
            {
                MinPostsForHashtags         = 1 * 1000 * 1000,
                ExploreTagsMinHashtagCount  = 0,
                ExploreTagsMinLikes         = 10,
                ExploreTagsMinCommentsCount = 0,
                MaxHashtagLength            = 30,
                MinHashtagLength            = 5,
                UserMinFollowerCount        = 1000,
                UserMinHashTagCount         = 5,
                UserMinCommentsCount        = 10,
                UserMinLikes                = 300
            };
            crawler = new CrawlerV4(requestHandler, settings);

            Console.WriteLine("GetExistingHumanoidTags start");
            db.GetAllHumanoidTags<HumanoidTag>();
            Console.WriteLine("GetExistingHumanoidTags finished");

            crawler.OnImageFound += image =>
            {
                Console.WriteLine(
                    "Image found -> [" + string.Join(", ", image.HumanoidTags.Take(5).Select(x => "#" + x)) + "... ]");
                UpsertImages.Add(image);
            };
            crawler.OnHashtagFoundComplete += hashtag =>
            {
                Console.WriteLine("Hashtag Found -> " + hashtag.Name + "(Posts:" + hashtag.Posts + ")");
                UpsertHtags.Add(hashtag);
            };
            crawler.OnHashtagNamesFound += hashtagNames =>
            {
                var enumerable = hashtagNames as string[] ?? hashtagNames.ToArray();
                foreach (var hashtagName in enumerable)
                {
                    var newHTag = new HumanoidTag { Name = hashtagName };
                    UpsertHtags.Add(newHTag);
                }
                Console.WriteLine("HashtagNames Found -> " + string.Join(", ", enumerable.Take(5).Select(x => "#" + x)) + "...");
            };

            new Thread(CrawlerStorageThread).Start();
            new Thread(Logs).Start();
            crawler.DoCrawling();
            startedDate = DateTime.Now;
        }

        private static void Logs()
        {
            while (true)
            {
                var debugInfos = crawler.GetDebugInfos();

                var requestCount = 0;
                var timespan = DateTime.Now - startedDate;
                var time = GetDateTimeFromTimespan(timespan);

                Console.Write("____");
                Console.Write($"PendingHtagsToSave: {UpsertHtags.Count} | ");
                Console.Write($"PendingImagesToSave: {UpsertImages.Count} | ");
                Console.Write($"SavedHtags: {savedHtagsCount} | ");
                Console.Write($"SavedImages: {savedImagesCount} | ");
                Console.Write($"Running since: {time} | ");
                Console.Write($"Hashtag: {debugInfos["hashtagsQueueCount"]} | ");
                Console.Write($"userQueue: {debugInfos["userQueueCount"]} | ");
                Console.Write($"imageQueue: {debugInfos["imageQueueCount"]} | ");
                Console.Write($"Requests: {requestCount} | ");
                Console.WriteLine("____");
                Thread.Sleep(1000);
            }
        }

        public static DateTime GetDateTimeFromTimespan(TimeSpan unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unixTimeStamp.TotalSeconds).ToLocalTime();
        }

        private static void CrawlerStorageThread()
        {
            while (true)
            {
                var countHTags = UpsertHtags.Count;
                var countImages = UpsertImages.Count;
                for (var i = countHTags - 1; i >= 0; i--)
                {
                    var htag = UpsertHtags[i];
                    Db.UpsertHumanoidTag(htag);
                    UpsertHtags.RemoveAt(i);
                    savedHtagsCount++;
                }
                for (var i = countImages - 1; i >= 0; i--)
                {
                    var image = UpsertImages[i];
                    try
                    {
                        Db.Upsert(image);
                        UpsertImages.RemoveAt(i);
                        savedImagesCount++;
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("!!!!!!!!!!!");
                        Console.WriteLine(e);
                        Console.WriteLine("!!!!!!!!!!!");
                    }
                }
                Db.Save();
                Thread.Sleep(100);
            }
        }
    }
}

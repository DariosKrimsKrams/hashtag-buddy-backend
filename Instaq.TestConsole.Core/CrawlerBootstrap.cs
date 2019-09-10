
namespace AutoTagger.TestConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V4;

    class CrawlerBootstrap
    {
        private static ICrawlerStorage Db;
        private static readonly List<IImage> Images = new List<IImage>();
        private static readonly List<IHumanoidTag> HumanoidTags = new List<IHumanoidTag>();
        private static int savedHtagsCount;
        private static int savedImagesCount;
        private static DateTime startedDate;
        private static CrawlerV4 crawler;

        public CrawlerBootstrap(ICrawlerStorage db)
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

            crawler.OnImageFound += image =>
            {
                Console.WriteLine(
                    "Image found -> [" + string.Join(", ", image.HumanoidTags.Take(5).Select(x => "#" + x)) + "... ]");
                Images.Add(image);
            };
            crawler.OnHashtagFoundComplete += hashtag =>
            {
                Console.WriteLine("Hashtag Found -> " + hashtag.Name + "(Posts:" + hashtag.Posts + ")");
                HumanoidTags.Add(hashtag);
            };
            crawler.OnHashtagNamesFound += hashtagNames =>
            {
                var enumerable = hashtagNames as string[] ?? hashtagNames.ToArray();
                foreach (var hashtagName in enumerable)
                {
                    var newHTag = new HumanoidTag { Name = hashtagName };
                    HumanoidTags.Add(newHTag);
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

                var (timing1, timing2, timing3) = GetTimings();

                Console.Write("____");
                Console.Write($"HTags (Saved/Remaining): {savedHtagsCount} / {HumanoidTags.Count} | ");
                Console.Write($"Images (Saved/Remaining): {savedImagesCount} / {Images.Count} | ");
                Console.Write($"Running since: {time.Hour}:{time.Minute}:{time.Second} | ");
                Console.Write($"Queues (Htag/User/Image): {debugInfos["hashtagsQueueCount"]} / {debugInfos["userQueueCount"]} / {debugInfos["imageQueueCount"]} | ");
                Console.Write($"RequestCount: {requestCount} | ");
                Console.Write($"Timings (Img/ Rels/Htags): {timing1} ms / {timing2} ms / {timing3} ms  | ");
                Console.WriteLine("____");
                Thread.Sleep(1000);
            }
        }

        private static (double, double, double) GetTimings()
        {
            double timing1 = 0;
            double timing2 = 0;
            double timing3 = 0;

            var timingImages = Db.GetTimings("images");
            var timingsRels = Db.GetTimings("rels");
            var timingsHTags = Db.GetTimings("htags");

            if (timingImages.Any())
            {
                timing1 = GetAverage(timingImages);
            }
            if (timingsRels.Any())
            {
                timing2 = GetAverage(timingsRels);
            }
            if (timingsHTags.Any())
            {
                timing3 = GetAverage(timingsHTags);
            }

            return (timing1, timing2, timing3);
        }

        private static double GetAverage(IEnumerable<TimeSpan> input)
        {
            return Math.Round(input.Select(x => x.TotalMilliseconds).Average(), 0);
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
                var countHTags  = HumanoidTags.Count;
                var countImages = Images.Count;

                SaveHumanoidTags(countHTags);
                SaveImages(countImages);

                if (countHTags == 0 && countImages == 0)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private static void SaveHumanoidTags(int countHTags)
        {
            if (countHTags == 0)
            {
                return;
            }

            var count = countHTags > 100 ? 100 : countHTags;
            var htagToUpdate = new IHumanoidTag[count];
            for (var i = count - 1; i >= 0; i--)
            {
                var htag = HumanoidTags[i];
                htagToUpdate[i] = htag;
                HumanoidTags.RemoveAt(i);
            }

            Db.InsertHumanoidTags(htagToUpdate);
            savedHtagsCount += count;
        }

        private static void SaveImages(int countImages)
        {
            if (countImages == 0)
            {
                return;
            }

            var count = countImages > 100 ? 100 : countImages;
            var imagesToUpdate = new IImage[count];
            for (var i = count - 1; i >= 0; i--)
            {
                var image = Images[i];
                imagesToUpdate[i] = image;
                Images.RemoveAt(i);
            }
            Db.InsertImages(imagesToUpdate);
            savedImagesCount += count;
        }
    }
}

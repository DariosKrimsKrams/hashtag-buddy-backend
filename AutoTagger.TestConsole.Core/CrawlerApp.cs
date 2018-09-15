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

    class CrawlerApp
    {
        private static ICrawlerStorage Db;
        private static readonly List<IImage> UpsertImages = new List<IImage>();
        private static readonly List<IHumanoidTag> UpsertHtags = new List<IHumanoidTag>();

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
            var crawler = new CrawlerV4(requestHandler, settings);

            Console.WriteLine("GetExistingHumanoidTags start");
            db.GetAllHumanoidTags<HumanoidTag>();
            Console.WriteLine("GetExistingHumanoidTags finished");

            crawler.OnImageFound += image =>
            {
                Console.WriteLine(
                    "Image found -> {\"user\":\"" + image.User.Username + "\", \"tags\": ["
                  + string.Join(", ", image.HumanoidTags.Select(x => "'" + x + "'").Skip(3)) + "]");
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
                Console.WriteLine("HashtagNames Found -> " + string.Join(", ", enumerable));
            };

            new Thread(CrawlerStorageThread).Start();
            crawler.DoCrawling();
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
                    Db.InsertOrUpdateHumanoidTag(htag);
                    UpsertHtags.RemoveAt(i);
                }
                for (var i = countImages - 1; i >= 0; i--)
                {
                    var image = UpsertImages[i];
                    try
                    {
                        Db.Upsert(image);
                        UpsertImages.RemoveAt(i);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("!!!!!!!!!!!");
                        Console.WriteLine(e);
                        Console.WriteLine("!!!!!!!!!!!");
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}

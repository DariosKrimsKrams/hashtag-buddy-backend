namespace AutoTagger.Crawler.V3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V3.Crawler;
    using AutoTagger.Crawler.V3.Queue;
    using static System.String;

    public class CrawlerV3 : ICrawler
    {
        private readonly Dictionary<string, int> settings;

        private readonly HashtagQueue<IHumanoidTag> hashtagQueue;
        private readonly UserQueue<string> userQueue;
        private readonly ShortcodeQueue<string> shortcodeQueue;

        private readonly RandomTagsCrawler randomTagsCrawler;
        private readonly ExploreTagsPageCrawler exploreTagsPagePageCrawler;
        private readonly ImageDetailPageCrawler imageDetailPageCrawler;
        private readonly UserPageCrawler userPageCrawler;

        public event Action<IHumanoidTag> OnHashtagFound;
        public event Action<IImage> OnImageFound;

        public CrawlerV3()
        {
            this.settings = new Dictionary<string, int>();
            this.settings.Add("MinPostsForHashtags", 1 * 1000 * 1000);
            this.settings.Add("limit", 10);

            this.hashtagQueue                = new HashtagQueue<IHumanoidTag>();
            this.userQueue      = new UserQueue<string>();
            this.shortcodeQueue = new ShortcodeQueue<string>();

            this.randomTagsCrawler           = new RandomTagsCrawler();
            this.exploreTagsPagePageCrawler  = new ExploreTagsPageCrawler(this);
            this.imageDetailPageCrawler      = new ImageDetailPageCrawler();
            this.userPageCrawler             = new UserPageCrawler();
        }

        public void DoCrawling(int limit, params string[] customTags)
        {
            this.BuildTags(customTags);
            this.hashtagQueue.Process(this.ExploreTagsCrawlerFunc);
        }

        public void BuildTags(string[] customTags)
        {
            var tags = customTags.Length == 0 ? this.randomTagsCrawler.Parse() : customTags;
            var hTags = new List<IHumanoidTag>();
            foreach (var name in tags)
            {
                hTags.Add(new HumanoidTag { Name = name });
            }
            this.hashtagQueue.EnqueueMultiple(hTags);
        }

        private void ExploreTagsCrawlerFunc(IHumanoidTag tag)
        {
            var url = $"https://www.instagram.com/explore/tags/{tag.Name}/";
            var (amountOfPosts, images) = this.exploreTagsPagePageCrawler.Parse(url);
            tag.Posts = amountOfPosts;
            this.OnHashtagFound?.Invoke(tag);

            var shortcodes = images.Select(x => x.Shortcode);
            this.shortcodeQueue.EnqueueMultiple(shortcodes);
            this.shortcodeQueue.Process(this.UserCrawlerFunc);
        }

        private void ImagePageCrawlerFunc(string shortcode)
        {
            var url = $"https://www.instagram.com/p/{shortcode}/?hl=en";
            var userName = this.imageDetailPageCrawler.Parse(url);

            this.userQueue.Enqueue(userName);
            this.userQueue.Process(this.UserCrawlerFunc);
        }

        private void UserCrawlerFunc(string user)
        {
            var url = $"https://www.instagram.com/{user}/?hl=en";
            var images = this.userPageCrawler.Parse(url);

            foreach (var image in images)
            {
                // ToDo limit check

                if (image == null)
                {
                    continue;
                }

                var hTagNames = image.HumanoidTags;
                foreach (var hTagName in hTagNames)
                {
                    var newHTag = new HumanoidTag
                    {
                        Name = hTagName
                    };
                    this.hashtagQueue.Enqueue(newHTag);
                }

                //var shortcode = (T)Convert.ChangeType(image.Shortcode, typeof(T));
                this.shortcodeQueue.Enqueue(image.Shortcode);
                //this.AddProcessed(shortcode);
                //yield return image;

                this.OnImageFound?.Invoke(image);
            }
        }

        public bool OverrideCondition(string key, int value)
        {
            if (this.settings.ContainsKey(key))
            {
                this.settings[key] = value;
                return true;
            }

            return false;
        }

        public int GetCondition(string key)
        {
            if (this.settings.ContainsKey(key))
            {
                return this.settings[key];
            }
            return 0;
        }
    }
}

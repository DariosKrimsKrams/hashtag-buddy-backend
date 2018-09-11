namespace AutoTagger.Crawler.V4
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V4.Crawler;
    using AutoTagger.Crawler.V4.Queue;
    using AutoTagger.Crawler.V4.Requests;

    public class CrawlerV4 : ICrawler
    {
        private readonly CrawlerSettings settings;

        private readonly HashtagQueue<IHumanoidTag> hashtagQueue;
        private readonly UserQueue<string> userQueue;
        private readonly ShortcodeQueue<string> shortcodeQueue;

        private readonly RandomTagsCrawler randomTagsCrawler;
        private readonly ExploreTagsPageHandler exploreTagsPagePageHandler;
        private readonly ImageDetailPageCrawler imageDetailPageCrawler;
        private readonly UserPageCrawler userPageCrawler;

        public event Action<IHumanoidTag> OnHashtagFoundComplete;
        public event Action<string> OnHashtagNameFound;
        public event Action<IImage> OnImageFound;

        public CrawlerV4(IRequestHandler requestHandler)
        {
            this.settings = new CrawlerSettings
            {
                MinPostsForHashtags         = 1 * 1000 * 1000,
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

            this.hashtagQueue   = new HashtagQueue<IHumanoidTag>();
            this.userQueue      = new UserQueue<string>();
            this.shortcodeQueue = new ShortcodeQueue<string>();

            this.randomTagsCrawler           = new RandomTagsCrawler(requestHandler);
            this.exploreTagsPagePageHandler  = new ExploreTagsPageHandler(this.settings, requestHandler);
            this.userPageCrawler             = new UserPageCrawler(this.settings, requestHandler);
            this.imageDetailPageCrawler      = new ImageDetailPageCrawler(this.settings, requestHandler);
        }

        public void DoCrawling(int limit, params string[] customTags)
        {
            this.InsertTags(customTags);
            this.hashtagQueue.Process(this.ExploreTagsCrawlerFunc, this.settings.LimitExplorePages);
        }

        public void InsertTags(string[] customTags)
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
            var (amountOfPosts, images) = this.exploreTagsPagePageHandler.Parse(url);
            tag.Posts = amountOfPosts;
            this.OnHashtagFoundComplete?.Invoke(tag);

            var shortcodes = images.Select(x => x.Shortcode);
            this.shortcodeQueue.EnqueueMultiple(shortcodes);
            this.shortcodeQueue.Process(this.ImagePageCrawlerFunc, this.settings.LimitImagePages);
        }

        private void ImagePageCrawlerFunc(string shortcode)
        {
            var url = $"https://www.instagram.com/p/{shortcode}/?hl=en";
            var username = this.imageDetailPageCrawler.ParseUsername(url);

            this.userQueue.Enqueue(username);
            this.userQueue.ProcessEachValueOnlyOnce = false;
            this.userQueue.Process(this.UserCrawlerFunc, this.settings.LimitUserPages);
        }

        private void UserCrawlerFunc(string username)
        {
            var url = $"https://www.instagram.com/{username}/?hl=en";
            var user = this.userPageCrawler.Parse(url);

            foreach (var image in user.Images)
            {
                image.Follower  = user.FollowerCount;
                image.Following = user.FollowingCount;
                image.Posts     = user.PostCount;

                this.hashtagQueue.EnqueueMultiple(image.HumanoidTags);
                var hTagNames = image.HumanoidTags;
                foreach (var hTagName in hTagNames)
                {
                    this.OnHashtagNameFound?.Invoke(hTagName);
                }

                this.OnImageFound?.Invoke(image);
            }
        }

        public void SetSetting(string key, int value)
        {
            switch (key)
            {
                case "LimitImagePages":
                    this.settings.LimitImagePages = value;
                    break;
                case "LimitUserPages":
                    this.settings.LimitUserPages = value;
                    break;
                case "LimitExplorePages":
                    this.settings.LimitExplorePages = value;
                    break;
            }
        }

    }
}

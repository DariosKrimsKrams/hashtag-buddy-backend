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
        private ICrawlerSettings settings;

        private readonly HashtagQueue<IHumanoidTag> hashtagQueue;
        private readonly BaseQueue<string> userQueue;
        private readonly BaseQueue<string> shortcodeQueue;

        private readonly RandomTagsCrawler randomTagsCrawler;
        private readonly ExploreTagsPageHandler exploreTagsPagePageHandler;
        private readonly ImageDetailPageCrawler imageDetailPageCrawler;
        private readonly UserPageCrawler userPageCrawler;

        public event Action<IHumanoidTag> OnHashtagFoundComplete;
        public event Action<IEnumerable<string>> OnHashtagNamesFound;
        public event Action<IImage> OnImageFound;

        public CrawlerV4(IRequestHandler requestHandler, ICrawlerSettings crawlerSetting)
        {
            this.settings = crawlerSetting;

            this.hashtagQueue   = new HashtagQueue<IHumanoidTag>();
            this.userQueue      = new BaseQueue<string>();
            this.shortcodeQueue = new BaseQueue<string>();

            this.randomTagsCrawler           = new RandomTagsCrawler(requestHandler);
            this.exploreTagsPagePageHandler  = new ExploreTagsPageHandler(this.settings, requestHandler);
            this.userPageCrawler             = new UserPageCrawler(this.settings, requestHandler);
            this.imageDetailPageCrawler      = new ImageDetailPageCrawler(this.settings, requestHandler);
        }

        public void DoCrawling(params string[] customTags)
        {
            this.InsertTags(customTags);
            this.hashtagQueue.Process(this.ExploreTagsCrawlerFunc, this.settings.LimitExplorePages);
        }

        public void InsertTags(string[] customTags)
        {
            var tags = customTags.Length == 0 ? this.randomTagsCrawler.Parse() : customTags;
            this.hashtagQueue.EnqueueMultiple(tags);
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
                this.hashtagQueue.EnqueueMultiple(image.HumanoidTags);
                this.OnHashtagNamesFound?.Invoke(image.HumanoidTags);
                this.OnImageFound?.Invoke(image);
            }
        }

        public void UpdateSettings(ICrawlerSettings settings)
        {
            this.settings = settings;
        }

    }
}

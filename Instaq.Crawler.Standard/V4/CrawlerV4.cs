﻿namespace Instaq.Crawler.V4
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Crawler.V4.Crawler;
    using Instaq.Crawler.V4.Queue;
    using Instaq.Crawler.V4.Requests;

    public class CrawlerV4 : ICrawler
    {
        private ICrawlerSettings settings;

        private readonly HashtagQueue<IHumanoidTag> hashtagQueue;
        private readonly BaseQueue<string> userQueue;
        private readonly BaseQueue<string> imageQueue;

        private readonly RandomTagsCrawler randomTagsCrawler;
        private readonly ExploreTagsPageHandler exploreTagsPageHandler;
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
            this.imageQueue     = new BaseQueue<string>();

            this.randomTagsCrawler           = new RandomTagsCrawler(requestHandler);
            this.exploreTagsPageHandler      = new ExploreTagsPageHandler(this.settings, requestHandler);
            this.userPageCrawler             = new UserPageCrawler(this.settings, requestHandler);
            this.imageDetailPageCrawler      = new ImageDetailPageCrawler(this.settings, requestHandler);
        }

        public void DoCrawling(params string[] customTags)
        {
            this.hashtagQueue.SetLimit(this.settings.LimitExplorePages);
            this.imageQueue.SetLimit(this.settings.LimitImagePages);
            this.userQueue.SetLimit(this.settings.LimitUserPages);

            this.InsertTags(customTags);
            new Thread(this.HashtagQueueThread).Start();
            new Thread(this.ShortcodeQueueThread).Start();
            new Thread(this.UserQueueThread).Start();
        }

        public void HashtagQueueThread()
        {
            this.hashtagQueue.Process(this.ExploreTagsCrawlerFunc, this.AllowedToRunExploreCrawler);
            Console.WriteLine("End hashtagQueue :(");
        }

        private bool AllowedToRunExploreCrawler()
        {
            return this.imageQueue.Count <= 10;
        }

        public void ShortcodeQueueThread()
        {
            this.imageQueue.Process(this.ImagePageCrawlerFunc, this.AllowedToRunImageCrawler);
            Console.WriteLine("End imageQueue :(");
        }

        private bool AllowedToRunImageCrawler()
        {
            return this.userQueue.Count <= 20;
        }

        public void UserQueueThread()
        {
            this.userQueue.Process(this.UserCrawlerFunc, this.AllowedToRunUserCrawler);
            Console.WriteLine("End userQueue :(");
        }

        private bool AllowedToRunUserCrawler()
        {
            return true;
        }

        public void InsertTags(string[] customTags)
        {
            var tags = customTags.Length == 0 ? this.randomTagsCrawler.Parse() : customTags;
            this.hashtagQueue.EnqueueMultiple(tags);
        }

        private void ExploreTagsCrawlerFunc(IHumanoidTag tag)
        {
            var url = $"https://www.instagram.com/explore/tags/{tag.Name}/";
            var (amountOfPosts, images) = this.exploreTagsPageHandler.Parse(url);
            tag.Posts = amountOfPosts;
            this.OnHashtagFoundComplete?.Invoke(tag);

            var shortcodes = images.Select(x => x.Shortcode);
            this.imageQueue.EnqueueMultiple(shortcodes);
        }

        private void ImagePageCrawlerFunc(string shortcode)
        {
            var url = $"https://www.instagram.com/p/{shortcode}/?hl=en";
            var username = this.imageDetailPageCrawler.ParseUsername(url);

            this.userQueue.Enqueue(username);
            this.userQueue.ProcessEachValueOnlyOnce = false;
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

        public IDictionary<string, int> GetDebugInfos()
        {
            var output = new Dictionary<string, int>();
            output.Add("hashtagsQueueCount", this.hashtagQueue.Count);
            output.Add("userQueueCount", this.userQueue.Count);
            output.Add("imageQueueCount", this.imageQueue.Count);
            return output;
        }

    }
}

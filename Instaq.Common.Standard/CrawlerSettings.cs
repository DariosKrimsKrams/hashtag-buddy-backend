namespace Instaq.Common
{
    using Instaq.Contract;

    public class CrawlerSettings : ICrawlerSettings
    {
        public int MinPostsForHashtags { get; set; }

        public int LimitExplorePages { get; set; }

        public int LimitImagePages { get; set; }

        public int LimitUserPages { get; set; }

        public int ExploreTagsMinHashtagCount { get; set; }

        public int ExploreTagsMinLikes { get; set; }

        public int ExploreTagsMinCommentsCount { get; set; }

        public int MaxHashtagLength { get; set; }

        public int MinHashtagLength { get; set; }

        public int UserMinFollowerCount { get; set; }

        public int UserMinHashTagCount { get; set; }

        public int UserMinCommentsCount { get; set; }

        public int UserMinLikes { get; set; }

    }
}

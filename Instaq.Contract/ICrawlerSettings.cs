namespace Instaq.Contract
{
    public interface ICrawlerSettings
    {
        int MinPostsForHashtags { get; set; }

        int LimitExplorePages { get; set; }

        int LimitImagePages { get; set; }

        int LimitUserPages { get; set; }

        int ExploreTagsMinHashtagCount { get; set; }

        int ExploreTagsMinLikes { get; set; }

        int ExploreTagsMinCommentsCount { get; set; }

        int MaxHashtagLength { get; set; }

        int MinHashtagLength { get; set; }

        int UserMinFollowerCount { get; set; }

        int UserMinHashTagCount { get; set; }

        int UserMinCommentsCount { get; set; }

        int UserMinLikes { get; set; }
    }
}
namespace AutoTagger.Crawler.V3.Crawler
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;

    class ExploreTagsPageCrawler : BaseImagePageCrawler
    {
        private readonly ICrawler crawler;

        public ExploreTagsPageCrawler(ICrawler crawler)
        {
            this.crawler = crawler;
            this.MinHashTagCount = 0;
            this.MinLikes        = 100;
        }

        public (int, IList<IImage>) Parse(string url)
        {
            var data = this.GetData(url);
            var amountPosts = GetAmountOfPosts(data);
            if (amountPosts < this.crawler.GetCondition("MinPostsForHashtags"))
            {
                return (amountPosts, new List<IImage>());
            }

            var nodes  = GetTopPostsNodes(data);
            var imagesList = this.GetImages(nodes) as IList<IImage>;

            return (amountPosts, imagesList);
        }

        private static int GetAmountOfPosts(dynamic data)
        {
            var hashtagNodes  = GetHashtagNodes(data);
            var amountPosts = Convert.ToInt32(hashtagNodes?.edge_hashtag_to_media?.count.ToString());
            return amountPosts;
        }

        public dynamic GetTopPostsNodes(dynamic data)
        {
            return data?.entry_data?.TagPage?[0]?.graphql?.hashtag?.edge_hashtag_to_top_posts?.edges;
        }

        private static dynamic GetHashtagNodes(dynamic data)
        {
            return data?.entry_data?.TagPage?[0]?.graphql?.hashtag;
        }
    }
}

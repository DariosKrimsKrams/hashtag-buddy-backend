namespace AutoTagger.Crawler.Standard.V1.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Contract;

    class ExploreTagsCrawler : ImageCrawler
    {
        private readonly ICrawler crawler;

        public ExploreTagsCrawler(ICrawler crawler)
        {
            this.crawler = crawler;
            this.MinHashTagCount = 0;
            this.MinLikes        = 100;
        }

        public (int, List<IImage>) Parse(string url)
        {
            var data = this.GetData(url);
            var amountPosts = GetAmountOfPosts(data);
            if (amountPosts < this.crawler.GetCondition("MinPostsForHashtags"))
            {
                return (amountPosts, null);
            }

            var nodes  = GetTopPostsNodes(data);
            var images = this.GetImages(nodes);
            var imagesList = images.ToList();

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

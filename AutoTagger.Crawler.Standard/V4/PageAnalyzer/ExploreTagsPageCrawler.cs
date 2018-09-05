namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Requests;

    public class ExploreTagsPageCrawler : BaseImagePageCrawler, IPageAnalyzer
    {

        public ExploreTagsPageCrawler(CrawlerSettings settings, IRequestHandler requestHandler)
        {
            this.Settings = settings;
            this.requestHandler = requestHandler;

            this.MinCommentsCount = this.Settings.ExploreTagsMinCommentsCount;
            this.MinHashTagCount  = this.Settings.ExploreTagsMinHashtagCount;
            this.MinLikes         = this.Settings.ExploreTagsMinLikes;
        }

        public (int, IList<IImage>) Parse(string url)
        {
            var data = this.GetData(url);
            var amountPosts = GetAmountOfPosts(data);
            if (amountPosts < this.Settings.MinPostsForHashtags)
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

        private dynamic GetTopPostsNodes(dynamic data)
        {
            return data?.entry_data?.TagPage?[0]?.graphql?.hashtag?.edge_hashtag_to_top_posts?.edges;
        }

        private static dynamic GetHashtagNodes(dynamic data)
        {
            return data?.entry_data?.TagPage?[0]?.graphql?.hashtag;
        }
    }
}

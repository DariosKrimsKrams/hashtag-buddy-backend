namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Requests;

    public class ExploreTagsPageHandler
    {
        private readonly ICrawlerSettings settings;
        private readonly ImagePageLogic imagePageLogic;

        public ExploreTagsPageHandler(ICrawlerSettings settings, IRequestHandler requestHandler)
        {
            this.settings = settings;
            this.imagePageLogic = new ImagePageLogic(settings, requestHandler);

            this.imagePageLogic.MinCommentsCount = this.settings.ExploreTagsMinCommentsCount;
            this.imagePageLogic.MinHashTagCount  = this.settings.ExploreTagsMinHashtagCount;
            this.imagePageLogic.MinLikes         = this.settings.ExploreTagsMinLikes;
        }

        public (int, IEnumerable<IImage>) Parse(string url)
        {
            var data = this.imagePageLogic.GetData(url);
            var amountPosts = GetAmountOfPosts(data);
            if (amountPosts < this.settings.MinPostsForHashtags)
            {
                return (amountPosts, new List<IImage>());
            }

            var nodes  = GetTopPostsNodes(data);
            var images = this.imagePageLogic.GetImages(nodes);
            images = this.imagePageLogic.RemoveUnrelevantImages(images);

            return (amountPosts, images);
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

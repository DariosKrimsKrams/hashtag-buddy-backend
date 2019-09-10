namespace Instaq.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Crawler.V4.Requests;

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
            var node = this.imagePageLogic.GetData(url);
            var hashtagNode = GetHashtagNodes(node);
            var amountPosts = GetAmountOfPosts(hashtagNode);
            if (amountPosts < this.settings.MinPostsForHashtags)
            {
                return (amountPosts, new List<IImage>());
            }

            var nodes  = GetTopPostsNodes(hashtagNode);
            var images = this.imagePageLogic.GetImages(nodes);
            images = this.imagePageLogic.RemoveUnrelevantImages(images);

            return (amountPosts, images);
        }

        private static dynamic GetHashtagNodes(dynamic node)
        {
            return node?.entry_data?.TagPage?[0]?.graphql?.hashtag;
        }

        private static int GetAmountOfPosts(dynamic hashtagNode)
        {
            var amountPosts = Convert.ToInt32(hashtagNode?.edge_hashtag_to_media?.count.ToString());
            return amountPosts;
        }

        private dynamic GetTopPostsNodes(dynamic hashtagNode)
        {
            return hashtagNode.edge_hashtag_to_top_posts?.edges;
        }

    }
}

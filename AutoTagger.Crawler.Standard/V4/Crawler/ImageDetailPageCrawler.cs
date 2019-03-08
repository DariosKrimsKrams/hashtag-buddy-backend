namespace AutoTagger.Crawler.V4.Crawler
{
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Crawler.Standard;
    using AutoTagger.Crawler.V4.Requests;

    public class ImageDetailPageCrawler
    {
        //private readonly ICrawlerSettings settings;
        private readonly IRequestHandler handler;
        private ImagePageLogic logic;

        public ImageDetailPageCrawler(ICrawlerSettings settings, IRequestHandler handler)
        {
            this.handler = handler;
            this.logic = new ImagePageLogic(settings, handler);

            this.logic.MinCommentsCount = settings.UserMinCommentsCount;
            this.logic.MinHashTagCount = settings.UserMinHashTagCount;
            this.logic.MinLikes = settings.UserMinLikes;
        }

        public string ParseUsername(string url)
        {
            var contentNode = this.GetContentNode(url);
            return GetUsername(contentNode);
        }

        public IImage ParseAll(string url)
        {
            var contentNode = this.GetContentNode(url);
            var image = this.logic.GetImage(contentNode);
            return image;
        }

        private dynamic GetContentNode(string url)
        {
            var node = this.handler.FetchNode(url);
            if (node == null)
            {
                return null;
            }
            return GetImageNode(node);
        }

        private static dynamic GetImageNode(dynamic node)
        {
            return node?.entry_data?.PostPage?[0]?.graphql?.shortcode_media;
        }

        private static dynamic GetUsername(dynamic node)
        {
            return node?.owner?.username;
        }
    }
}

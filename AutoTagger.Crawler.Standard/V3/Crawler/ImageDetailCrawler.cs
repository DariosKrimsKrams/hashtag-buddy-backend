namespace AutoTagger.Crawler.V3.Crawler
{
    using AutoTagger.Crawler.Standard;

    public class ImageDetailCrawler : HttpCrawler
    {
        public string Parse(string url)
        {
            var document   = this.FetchDocument(url);
            var scriptNode = GetScriptNodeData(document);
            return GetImageNode(scriptNode);
        }

        private static dynamic GetImageNode(dynamic data)
        {
            return data?.entry_data?.PostPage?[0]?.graphql?.shortcode_media?.owner?.username.ToString();
        }
    }
}

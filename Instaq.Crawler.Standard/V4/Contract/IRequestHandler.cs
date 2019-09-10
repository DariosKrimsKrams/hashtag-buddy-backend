namespace AutoTagger.Crawler.V4.Requests
{
    using HtmlAgilityPack;

    public interface IRequestHandler
    {
        HtmlNode FetchDocument(string url);

        dynamic FetchNode(string url);
    }
}

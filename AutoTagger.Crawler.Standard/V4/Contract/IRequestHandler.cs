namespace AutoTagger.Crawler.V4.Requests
{
    public interface IRequestHandler
    {
        dynamic FetchNode(string url);
    }
}

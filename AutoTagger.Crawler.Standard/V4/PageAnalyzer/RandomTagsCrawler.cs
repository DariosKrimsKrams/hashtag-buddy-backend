namespace AutoTagger.Crawler.V4.Crawler
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Crawler.Standard;

    public class RandomTagsCrawler : HttpRequestHandler
    {
        public IEnumerable<string> Parse()
        {
            // or take this url: https://top-hashtags.com/random/
            var url = "https://www.all-hashtag.com/library/contents/ajax_top.php";
            var document = this.FetchDocument(url);
            var nodes = document.SelectNodes("//section[@id='tab1']//span[@class='hashtag']");
            return nodes.Select(n => n.InnerText.Trim(' ', '#'));
        }
    }
}

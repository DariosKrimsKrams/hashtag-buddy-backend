namespace Instaq.Crawler.V4.Crawler
{
    using System.Collections.Generic;
    using System.Linq;
    using Instaq.Crawler.Standard;
    using Instaq.Crawler.V4.Requests;

    public class RandomTagsCrawler
    {
        private readonly IRequestHandler requestHandler;

        public RandomTagsCrawler(IRequestHandler requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public IEnumerable<string> Parse()
        {
            // or take this url: https://top-hashtags.com/random/
            var url = "https://www.all-hashtag.com/library/contents/ajax_top.php";
            var document = this.requestHandler.FetchDocument(url);
            var nodes = document.SelectNodes("//section[@id='tab1']//span[@class='hashtag']");
            return nodes.Select(n => n.InnerText.Trim(' ', '#').ToLower());
        }
    }
}

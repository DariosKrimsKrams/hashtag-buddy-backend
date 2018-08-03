namespace AutoTagger.Crawler.V3.Crawler
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Crawler.Standard;

    public class RandomTagsCrawler : HttpCrawler
    {
        public IEnumerable<string> Parse()
        {
            // or take this url: https://top-hashtags.com/random/
            var url = "https://www.all-hashtag.com/library/contents/ajax_top.php";
            var document = this.FetchDocument(url);

            // <section id="tab1" class="tab"><h4 class="tab-title">Top 100 hashtags <span class="color-brand">today</span></h4><span class="hashtag">#look</span>
            var nodes = document.SelectNodes("//section[@id='tab1']//span[@class='hashtag']");
            return nodes.Select(n => n.InnerText.Trim(' ', '#'));
        }
    }
}

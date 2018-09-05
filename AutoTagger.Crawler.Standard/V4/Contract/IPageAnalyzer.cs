namespace AutoTagger.Crawler.V4.Crawler
{
    using System.Collections.Generic;

    using AutoTagger.Contract;

    public interface IPageAnalyzer
    {
        (int, IList<IImage>) Parse(string url);
    }
}
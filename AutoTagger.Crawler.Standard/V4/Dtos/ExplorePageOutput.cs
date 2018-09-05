namespace AutoTagger.Crawler.V4.Dtos
{
    using System.Collections.Generic;

    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Contract;

    public class ExplorePageOutput : IOutput
    {
        public int AmountOfPosts { get; set; }

        public IList<IImage> Images { get; set; }
    }
}

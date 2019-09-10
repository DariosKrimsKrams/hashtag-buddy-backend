namespace Instaq.Crawler.V4.Dtos
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;
    using Instaq.Crawler.V4.Contract;

    public class ExplorePageOutput : IOutput
    {
        public int AmountOfPosts { get; set; }

        public IList<IImage> Images { get; set; }
    }
}

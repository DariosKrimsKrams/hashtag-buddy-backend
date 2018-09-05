using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Crawler.V4
{
    public class CrawlerSettings
    {
        public int MinPostsForHashtags { get; set; }

        public int Limit { get; set; }

        public int ExploreTagsMinHashtagCount { get; set; }

        public int ExploreTagsMinLikes { get; set; }

        public int ExploreTagsMinCommentsCount { get; set; }

        public int MaxHashtagLength { get; set; }

        public int MinHashtagLength { get; set; }

    }
}

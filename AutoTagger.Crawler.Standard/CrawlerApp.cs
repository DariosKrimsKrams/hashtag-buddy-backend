namespace AutoTagger.Crawler.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Database.Standard;

    public class CrawlerApp
    {
        private readonly ICrawlerStorage db;
        private readonly List<IHumanoidTag> allHTags;
        private readonly ICrawler crawler;

        public CrawlerApp(ICrawlerStorage db, ICrawler crawler)
        {
            this.db                     =  db;
            this.crawler                =  crawler;
            this.allHTags               =  db.GetAllHumanoidTags<HumanoidTag>().ToList();
            this.crawler.OnHashtagFound += this.HashtagFound;
        }

        public event Action<IImage> OnImageSaved;

        public void DoCrawling(int limit, params string[] customTags)
        {
            var images = this.crawler.DoCrawling(limit, customTags);

            foreach (var image in images)
            {
                foreach (var hTagName in image.HumanoidTags)
                {
                    var exists = this.allHTags.FirstOrDefault(htag => htag.Name == hTagName);
                    if (exists != null)
                        continue;
                    var newHTag = new HumanoidTag { Name = hTagName };
                    this.db.InsertOrUpdateHumanoidTag(newHTag);
                    this.allHTags.Add(newHTag);
                }

                this.db.Upsert(image);
                this.ImageFound(image);
            }
        }

        private void HashtagFound(IHumanoidTag hTag)
        {
            this.db.InsertOrUpdateHumanoidTag(hTag);
            var exists = this.allHTags.FirstOrDefault(htag => htag.Name == hTag.Name);
            if (exists == null)
                this.allHTags.Add(hTag);
        }

        private void ImageFound(IImage image)
        {
            this.OnImageSaved?.Invoke(image);
        }
    }
}

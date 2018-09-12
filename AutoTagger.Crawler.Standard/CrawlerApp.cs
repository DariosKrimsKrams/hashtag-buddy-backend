//namespace AutoTagger.Crawler.Standard
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using AutoTagger.Common;
//    using AutoTagger.Contract;

//    public class CrawlerApp
//    {
//        private readonly ICrawlerStorage db;
//        private readonly List<IHumanoidTag> allHTags;
//        private readonly ICrawler crawler;

//        public CrawlerApp(ICrawlerStorage db, ICrawler crawler)
//        {
//            this.db                     =  db;
//            this.crawler                =  crawler;
//            this.allHTags               =  db.GetAllHumanoidTags<HumanoidTag>().ToList();
//            this.crawler.OnHashtagFoundComplete += this.HashtagFound;
//            this.crawler.OnImageFound += this.ImageFound;
//        }

//        public event Action<IHumanoidTag> OnHashtagFound;
//        public event Action<IImage> OnImageSaved;

//        public void DoCrawling(params string[] customTags)
//        {
//            this.crawler.DoCrawling(customTags);
//        }

//        private void HashtagFound(IHumanoidTag humanoidTag)
//        {
//            this.db.InsertOrUpdateHumanoidTag(humanoidTag);
//            //var existinhHumanoidTag = this.allHTags.FirstOrDefault(htag => htag.Name == humanoidTag.Name);
//            //if (existinhHumanoidTag == null)
//            //{
//            //    this.allHTags.Add(humanoidTag);
//            //}
//            //else
//            //{
//            //    existinhHumanoidTag.Posts = humanoidTag.Posts;
//            //}
//            this.OnHashtagFound?.Invoke(humanoidTag);
//        }

//        private void ImageFound(IImage image)
//        {
//            foreach (var hTagName in image.HumanoidTags)
//            {
//                var exists = this.allHTags.FirstOrDefault(htag => htag.Name == hTagName && htag.Posts != 0);
//                if (exists != null)
//                    continue;
//                var newHTag = new HumanoidTag { Name = hTagName };
//                this.db.InsertOrUpdateHumanoidTag(newHTag);
//                this.allHTags.Add(newHTag);
//            }

//            this.db.Upsert(image);
//            this.ImageFound(image);


//            this.OnImageSaved?.Invoke(image);
//        }
//    }
//}

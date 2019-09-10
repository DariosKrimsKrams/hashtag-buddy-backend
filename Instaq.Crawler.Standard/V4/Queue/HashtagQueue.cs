namespace AutoTagger.Crawler.V4.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;

    public class HashtagQueue<T> : BaseQueue<T>
    {
        public void EnqueueMultiple(IEnumerable<string> tagNames)
        {
            foreach (var tagName in tagNames)
            {
                var htag = new HumanoidTag { Name = tagName };
                var htagAsT = (T)Convert.ChangeType(htag, typeof(HumanoidTag));
                this.Enqueue(htagAsT);
            }
        }

        protected bool Contains(T checkingTag)
        {
            var checkingHTag = (HumanoidTag) Convert.ChangeType(checkingTag, typeof(HumanoidTag));
            var exists = this.FirstOrDefault(
                htag => ((HumanoidTag) Convert.ChangeType(htag, typeof(HumanoidTag))).Name == checkingHTag.Name);
            return exists != null;
        }

        protected new bool IsProcessed(T checkingTag)
        {
            var checkingHTag = (HumanoidTag) Convert.ChangeType(checkingTag, typeof(HumanoidTag));
            foreach (var htag in this.Processed)
            {
                var newHTag = (HumanoidTag) Convert.ChangeType(htag, typeof(HumanoidTag));
                if (newHTag.Name == checkingHTag.Name && newHTag.Posts != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

namespace AutoTagger.Crawler.V3.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using AutoTagger.Common;
    using AutoTagger.Contract;

    public class HashtagQueue<T> : BaseQueue<T>
    {

        private bool IsProcessed(T checkingTag)
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

        private bool Contains(T checkingTag)
        {
            var checkingHTag = (HumanoidTag)Convert.ChangeType(checkingTag, typeof(HumanoidTag));
            var exists = this.FirstOrDefault(htag => ((HumanoidTag)Convert.ChangeType(htag, typeof(HumanoidTag))).Name == checkingHTag.Name);
            return exists != null;
        }

        public void AddProcessed(IEnumerable<T> tags)
        {
            foreach (var tag in tags)
            {
                this.Processed.Add(tag);
            }
        }

    }
}

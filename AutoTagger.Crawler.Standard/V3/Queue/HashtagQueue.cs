namespace AutoTagger.Crawler.V3.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using AutoTagger.Common;
    using AutoTagger.Contract;

    class HashtagQueue<T> : ConcurrentQueue<T>
    {
        private readonly HashSet<T> processed;
        private readonly ShortcodeQueue<string> shortcodeQueue;

        public event Action<T> OnHashtagFound;

        public bool AllowEnqueue = true;

        public HashtagQueue()
        {
            this.processed = new HashSet<T>();
            this.shortcodeQueue = new ShortcodeQueue<string>();
        }

        public void Build(IEnumerable<T> hashTags)
        {
            foreach (var tag in hashTags)
            {
                this.Enqueue(tag);
            }
        }

        public IEnumerable<IImage> Process(Func<T, (int, List<string>)> shortcodesCrawling,
                                           Func<string, string> imagePageCrawling,
                                           Func<string, IEnumerable<IImage>> userPageCrawling
            )
        {
            while (this.TryDequeue(out T currentTag))
            {
                if (this.IsTagProcessed(currentTag))
                {
                    continue;
                }

                var (amountPosts, shortcodes) = shortcodesCrawling(currentTag);

                SetAmountOfPosts(currentTag, amountPosts);
                this.AddProcessed(currentTag);
                this.HashtagFound(currentTag);

                this.shortcodeQueue.Build(shortcodes);

                var images = this.shortcodeQueue.Process(imagePageCrawling, userPageCrawling);

                foreach (var image in images)
                {
                    // limit check
                    if (image == null)
                    {
                        yield break;
                    }

                    var hTagNames = image.HumanoidTags;
                    foreach (var hTagName in hTagNames)
                    {
                        var newHTag = new HumanoidTag
                        {
                            Name = hTagName
                        };
                        var hTagAsT = (T)Convert.ChangeType(newHTag, typeof(HumanoidTag));
                        this.Enqueue(hTagAsT);
                    }

                    yield return image;
                }
            }
        }

        private void HashtagFound(T tag)
        {
            this.OnHashtagFound?.Invoke(tag);
        }

        private static void SetAmountOfPosts(T currentTag, int amountPosts)
        {
            Type         hTagType = currentTag.GetType();
            PropertyInfo pinfo    = hTagType.GetProperty("Posts");
            pinfo.SetValue(currentTag, amountPosts, null);
        }

        private new void Enqueue(T tag)
        {
            if (tag == null
                || this.IsTagProcessed(tag)
                || this.Contains(tag)
                || !this.AllowEnqueue)
            {
                return;
            }

            base.Enqueue(tag);
        }

        private bool IsTagProcessed(T checkingTag)
        {
            var checkingHTag = (HumanoidTag) Convert.ChangeType(checkingTag, typeof(HumanoidTag));
            foreach (var htag in this.processed)
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
                this.processed.Add(tag);
            }
        }

        private void AddProcessed(T tag)
        {
            this.processed.Add(tag);
        }

        public void SetLimit(int limit)
        {
            this.shortcodeQueue.SetLimit(limit);
        }

    }
}

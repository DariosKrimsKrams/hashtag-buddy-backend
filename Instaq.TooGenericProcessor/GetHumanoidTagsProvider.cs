namespace Instaq.TooGenericProcessor
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    class GetHumanoidTagsProvider
    {
        private readonly ITooGenericStorage storage;
        private readonly ConcurrentQueue<IHumanoidTag> queue;
        private int limitSkip;
        private const int AmountToSelect = 200;
        private const int LimitToGetMore = 50;

        public GetHumanoidTagsProvider(ITooGenericStorage storage)
        {
            this.storage = storage;
            this.queue = new ConcurrentQueue<IHumanoidTag>();
        }

        public IHumanoidTag GetNextHumanoidTag(bool onlyWithoutRefCountYet)
        {
            if (this.queue.Count < LimitToGetMore)
            {
                this.GetAllHumanoidTags(onlyWithoutRefCountYet);
            }

            this.queue.TryDequeue(out var hTag);
            return hTag;
        }

        private void GetAllHumanoidTags(bool onlyWithoutRefCountYet)
        {
            Console.WriteLine("GetHumanoidTags limitSkip=" + this.limitSkip);

            IEnumerable<IHumanoidTag> hTags = null;
            if (onlyWithoutRefCountYet)
            {
                hTags = this.storage.GetHumanoidTagsWithNoRefCount(AmountToSelect, this.limitSkip);
            }
            else
            {
                hTags = this.storage.GetHumanoidTags(AmountToSelect, this.limitSkip);
            }
            foreach (var hTag in hTags)
            {
                this.queue.Enqueue(hTag);
                this.limitSkip++;
            }
        }

    }
}

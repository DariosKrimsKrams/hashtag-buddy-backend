namespace Instaq.TooGenericProcessor
{
    using System;
    using System.Collections.Concurrent;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    class GetHumanoidTagsProvider
    {
        private readonly ITooGenericStorage storage;
        private readonly ConcurrentQueue<IHumanoidTag> queue;
        private int limitSkip;

        public GetHumanoidTagsProvider(ITooGenericStorage storage)
        {
            this.storage = storage;
            this.queue = new ConcurrentQueue<IHumanoidTag>();
        }

        public IHumanoidTag GetNextHumanoidTag()
        {
            if (this.queue.Count < 10)
            {
                this.GetHumanoidTags();
            }

            this.queue.TryDequeue(out var hTag);
            return hTag;
        }

        private void GetHumanoidTags()
        {
            Console.WriteLine("GetHumanoidTags limitSkip=" + this.limitSkip);

            var count = 100;
            var hTags = this.storage.GetHumanoidTags(count, this.limitSkip);
            foreach (var hTag in hTags)
            {
                this.queue.Enqueue(hTag);
                this.limitSkip++;
            }
        }
    }
}

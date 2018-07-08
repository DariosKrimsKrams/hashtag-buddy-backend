namespace Instaq.TooGenericProcessor
{
    using System.Collections.Concurrent;
    using AutoTagger.Contract;

    class HashtagProvider
    {
        private readonly ITooGenericStorage storage;
        private readonly ConcurrentQueue<IHumanoidTag> queue;
        private int lastId;

        public HashtagProvider(ITooGenericStorage storage)
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
            var count = 100;
            var hTags = storage.GetHumanoidTags(count, this.lastId);
            foreach (var hTag in hTags)
            {
                this.queue.Enqueue(hTag);
                this.lastId = hTag.Id;
            }
        }
    }
}

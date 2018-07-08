namespace Instaq.TooGenericProcessor
{
    using System;
    using System.Collections.Concurrent;
    using AutoTagger.Contract;

    class GetHumanoidTagsProvider
    {
        private readonly ITooGenericStorage storage;
        private readonly ConcurrentQueue<IHumanoidTag> queue;
        private int lastId;

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
            Console.WriteLine("GetHumanoidTags lastId=" + this.lastId);

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

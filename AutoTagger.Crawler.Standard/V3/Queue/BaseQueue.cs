namespace AutoTagger.Crawler.V3.Queue
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class BaseQueue<T> : ConcurrentQueue<T>
    {
        protected readonly HashSet<T> Processed = new HashSet<T>();

        public void Process(Action<T> func)
        {
            while (this.GetEntry(out T currentShortcode))
            {
                func(currentShortcode);
            }
        }

        public new void Enqueue(T entry)
        {
            if (entry == null || this.IsProcessed(entry) || this.Contains(entry))
            {
                return;
            }

            base.Enqueue(entry);
        }

        public void EnqueueMultiple(IEnumerable<T> entries)
        {
            foreach (var entry in entries)
            {
                this.Enqueue(entry);
            }
        }

        public bool GetEntry(out T entry)
        {
            var status = this.TryDequeue(out T entry2);
            entry = entry2;
            if (!status)
            {
                return false;
            }

            if (this.IsProcessed(entry2))
            {
                entry = entry2;
                return this.GetEntry(out entry);
            }

            this.AddProcessed(entry);
            return true;
        }

        private void AddProcessed(T value)
        {
            this.Processed.Add(value);
        }

        private bool IsProcessed(T value)
        {
            return this.Processed.Contains(value);
        }
    }
}

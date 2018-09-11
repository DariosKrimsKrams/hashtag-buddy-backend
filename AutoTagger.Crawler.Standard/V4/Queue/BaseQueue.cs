namespace AutoTagger.Crawler.V4.Queue
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class BaseQueue<T> : ConcurrentQueue<T>
    {
        protected readonly HashSet<T> Processed = new HashSet<T>();

        private int count;

        public bool ProcessEachValueOnlyOnce = true;

        public void Process(Action<T> func, int limit = 0)
        {
            while (this.GetEntry(out T value))
            {
                func(value);
                this.count++;
                if (limit > 0 && this.count >= limit)
                {
                    break;
                }
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

        protected void AddProcessed(T value)
        {
            this.Processed.Add(value);
        }

        protected bool IsProcessed(T value)
        {
            if (!this.ProcessEachValueOnlyOnce)
            {
                return false;
            }
            return this.Processed.Contains(value);
        }
    }
}

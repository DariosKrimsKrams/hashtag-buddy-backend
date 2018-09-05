namespace Instaq.Crawler.Tests
{
    using System.Collections.Generic;
    using AutoTagger.Crawler.V3.Queue;
    using NUnit.Framework;

    class Queue_WhenEnqueue
    {
        private BaseQueue<string> queue;

        [SetUp]
        public void Setup()
        {
            this.queue = new BaseQueue<string>();
        }

        [Test]
        public void ThenNoInserts_ShouldBeEmptyQueue()
        {
            Assert.IsTrue(this.queue.Count == 0);
        }

        [Test]
        public void ThenInsertedEntry_ShouldBeOneEntryQueue()
        {
            this.queue.Enqueue("test");
            Assert.IsTrue(this.queue.Count == 1);
        }

        [Test]
        public void ThenInsertedTwice_ShouldNotBeDuplicatesInQueue()
        {
            this.queue.Enqueue("test");
            this.queue.Enqueue("test");
            Assert.IsTrue(this.queue.Count == 1);
        }

        [Test]
        public void ThenInsertedDifferentSingleEntries_ShouldAllBeInQueue()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");
            this.queue.Enqueue("test3");
            Assert.IsTrue(this.queue.Count == 3);
        }
    }
}

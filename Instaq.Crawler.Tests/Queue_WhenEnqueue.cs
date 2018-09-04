namespace Instaq.Crawler.Tests
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using AutoTagger.Contract;
    using AutoTagger.Crawler.V3;
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
        public void ThenInsertMultiple_ShouldAllBeInQueue()
        {
            var entries = new List<string> { "test1", "test2", "test3" };
            this.queue.EnqueueMultiple(entries);
            Assert.IsTrue(this.queue.Count == 3);
        }

        [Test]
        public void ThenInsertedDifferentSingleEntries_ShouldAllBeInQueue()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");
            this.queue.Enqueue("test3");
            Assert.IsTrue(this.queue.Count == 3);
        }

        [Test]
        public void ThenInsertedMultipleAndSingleEntries_ShouldAllBeInQueue()
        {
            this.queue.Enqueue("test0");
            var entries = new List<string> { "test1", "test2", "test3" };
            this.queue.EnqueueMultiple(entries);
            this.queue.Enqueue("test4");
            Assert.IsTrue(this.queue.Count == 5);
        }

        [Test]
        public void ThenInsertedMultipleAndSingleEntriesDublicates_ShouldBeNoDuplicatesInQueue()
        {
            this.queue.Enqueue("test1");
            var entries = new List<string> { "test1", "test2" };
            this.queue.EnqueueMultiple(entries);
            var entries2 = new List<string> { "test2", "test3" };
            this.queue.EnqueueMultiple(entries2);
            Assert.IsTrue(this.queue.Count == 3);
        }
    }
}

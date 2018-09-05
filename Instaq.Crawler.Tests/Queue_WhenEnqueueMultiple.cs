namespace Instaq.Crawler.Tests
{
    using System.Collections.Generic;
    using AutoTagger.Crawler.V3.Queue;
    using NUnit.Framework;

    class Queue_WhenEnqueueMultiple
    {
        private BaseQueue<string> queue;

        [SetUp]
        public void Setup()
        {
            this.queue = new BaseQueue<string>();
        }

        [Test]
        public void ThenInsertMultiple_ShouldAllBeInQueue()
        {
            var entries = new List<string> { "test1", "test2", "test3" };
            this.queue.EnqueueMultiple(entries);
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

namespace Instaq.Crawler.Tests.Queue
{
    using System.Collections.Generic;
    using AutoTagger.Crawler.V4.Queue;
    using NUnit.Framework;

    class BaseQueue_WhenEnqueueMultiple
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
            var entries = new List<string> { "test1", "test2", "test3" };

            this.queue.Enqueue("test0");
            this.queue.EnqueueMultiple(entries);
            this.queue.Enqueue("test4");

            var count = this.queue.Count;

            Assert.IsTrue(count == 5);
        }

        [Test]
        public void ThenInsertedMultipleAndSingleEntriesDublicates_ShouldBeNoDuplicatesInQueue()
        {
            var entries = new List<string> { "test1", "test2" };
            var entries2 = new List<string> { "test2", "test3" };

            this.queue.Enqueue("test1");
            this.queue.EnqueueMultiple(entries);
            this.queue.EnqueueMultiple(entries2);

            var count = this.queue.Count;

            Assert.IsTrue(count == 3);
        }

    }
}

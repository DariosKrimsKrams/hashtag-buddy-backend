namespace Instaq.Crawler.Tests.Queue
{
    using System.Collections.Generic;
    using Instaq.Crawler.V4.Queue;
    using NUnit.Framework;

    class BaseQueue_WhenEnqueue
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
            var count = this.queue.Count;

            Assert.IsTrue(count == 0);
        }

        [Test]
        public void ThenInsertedEntry_ShouldBeOneEntryQueue()
        {
            this.queue.Enqueue("test");

            var count = this.queue.Count;

            Assert.IsTrue(count == 1);
        }

        [Test]
        public void ThenInsertedTwice_ShouldNotBeDuplicatesInQueue()
        {
            this.queue.Enqueue("test");
            this.queue.Enqueue("test");

            var count = this.queue.Count;

            Assert.IsTrue(count == 1);
        }

        [Test]
        public void ThenInsertedDifferentSingleEntries_ShouldAllBeInQueue()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");
            this.queue.Enqueue("test3");

            var count = this.queue.Count;

            Assert.IsTrue(count == 3);
        }
    }
}

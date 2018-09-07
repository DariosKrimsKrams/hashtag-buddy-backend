namespace Instaq.Crawler.Tests.Queue
{
    using AutoTagger.Crawler.V4.Queue;
    using NUnit.Framework;

    class BaseQueue_WhenGettingEntry
    {
        private BaseQueue<string> queue;

        [SetUp]
        public void Setup()
        {
            this.queue = new BaseQueue<string>();
        }

        [Test]
        public void ThenInsertedValue_ShouldGetBack()
        {
            this.queue.Enqueue("test1");

            this.queue.GetEntry(out var entry);

            Assert.AreEqual(entry, "test1");
        }

        [Test]
        public void ThenFirstInsertedValue_ShouldGetBackFirst()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");

            this.queue.GetEntry(out var entry);

            Assert.AreEqual(entry, "test1");
        }

        [Test]
        public void ThenAllInsertedValue_ShouldGetBackInItsOrder()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");
            this.queue.Enqueue("test3");

            this.queue.GetEntry(out var entry);
            this.queue.GetEntry(out var entry2);
            this.queue.GetEntry(out var entry3);

            Assert.AreEqual(entry, "test1");
            Assert.AreEqual(entry2, "test2");
            Assert.AreEqual(entry3, "test3");
        }

        [Test]
        public void ThenDuplicateInserts_ShouldReturnOnlyOnce()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");
            this.queue.Enqueue("test1");
            this.queue.GetEntry(out var entry);
            this.queue.GetEntry(out var entry2);
            var status = this.queue.GetEntry(out var entry3);

            Assert.AreEqual(entry, "test1");
            Assert.AreEqual(entry2, "test2");
            Assert.AreEqual(entry3, null);
            Assert.IsFalse(status);
        }

        [Test]
        public void ThenNoEntriesInserted_ShouldReturnNothing()
        {
            this.queue.GetEntry(out var entry);

            Assert.AreEqual(entry, null);
        }

        [Test]
        public void ThenMoreEntriesTryToGetThanInserted_ShouldReturnNothingAfterAllReturned()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");

            this.queue.GetEntry(out _);
            this.queue.GetEntry(out _);
            this.queue.GetEntry(out var entry3);

            Assert.AreEqual(entry3, null);
        }

        [Test]
        public void ThenReturnBoolean_ShouldBeTrueIfQueueFilled()
        {
            this.queue.Enqueue("test1");

            var status = this.queue.GetEntry(out _);

            Assert.IsTrue(status);
        }

        [Test]
        public void ThenReturnBoolean_ShouldBeFalseIfQueueEmpty()
        {
            var status = this.queue.GetEntry(out _);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenReturnBoolean_ShouldBeFalseIfAllEntriesGone()
        {
            this.queue.Enqueue("test1");
            this.queue.Enqueue("test2");

            this.queue.GetEntry(out _);
            this.queue.GetEntry(out _);
            var status = this.queue.GetEntry(out _);

            Assert.IsFalse(status);
        }

        [Test]
        public void ThenProcessedValue_ShouldNotPossibleToEnqueueAgain()
        {
            this.queue.Enqueue("test1");
            this.queue.GetEntry(out _);
            this.queue.Enqueue("test1");

            var status = this.queue.GetEntry(out var entry);

            Assert.AreEqual(entry, null);
            Assert.IsFalse(status);
        }
    }
}

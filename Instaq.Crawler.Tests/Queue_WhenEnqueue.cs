namespace Instaq.Crawler.Tests
{
    using System.Collections.Concurrent;

    using AutoTagger.Contract;
    using AutoTagger.Crawler.V3;
    using AutoTagger.Crawler.V3.Queue;

    using NUnit.Framework;

    class Queue_WhenEnqueue
    {
        private ConcurrentQueue<string> Queue;

        [SetUp]
        public void Setup()
        {
            this.Queue = new HashtagQueue<string>();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}

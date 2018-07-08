using System;

namespace Instaq.TooGenericProcessor
{
    using AutoTagger.Contract;

    public class TooGenericProcessor
    {
        private ITooGenericStorage storage;

        private HashtagProvider provider;

        public TooGenericProcessor(ITooGenericStorage storage)
        {
            this.storage = storage;
            provider = new HashtagProvider(storage);
        }

        public void Start()
        {
            var hTag = this.provider.GetNextHumanoidTag();
        }
    }
}

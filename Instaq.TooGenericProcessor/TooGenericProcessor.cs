namespace Instaq.TooGenericProcessor
{
    using System;
    using AutoTagger.Contract;

    public class TooGenericProcessor
    {
        private GetHumanoidTagsProvider provider;

        private ITooGenericStorage storage;

        public TooGenericProcessor(ITooGenericStorage storage)
        {
            this.storage  = storage;
            this.provider = new GetHumanoidTagsProvider(storage);
        }

        public void CalcHumanoidTagsRefCount(bool onlyWithoutRefCountYet=false)
        {
            while (true)
            {
                var hTag = this.provider.GetNextHumanoidTag(onlyWithoutRefCountYet);
                if (hTag == null)
                {
                    Console.WriteLine("Exit");
                    break;
                }

                var count = this.storage.CountHumanoidTagsForHumanoidTag(hTag.Name);
                hTag.RefCount = count;
                Console.WriteLine(hTag.Name + " -> " + count);
                this.storage.UpdateRefCount(hTag);
            }
        }
    }
}

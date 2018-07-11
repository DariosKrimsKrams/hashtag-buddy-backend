namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface ITooGenericStorage
    {
        IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0);

        int CountHumanoidTagsForHumanoidTag(string name);

        void UpdateRefCount(IHumanoidTag humanoidTag);
    }
}

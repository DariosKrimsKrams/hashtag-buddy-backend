namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface ITooGenericStorage
    {
        IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0);

        int CountHumanoidTagsForHumanoidTag(string name);

        void UpdateRefCount(IHumanoidTag humanoidTag);
    }
}

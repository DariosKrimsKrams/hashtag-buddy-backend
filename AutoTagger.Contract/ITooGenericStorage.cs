namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface ITooGenericStorage
    {
        IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int limitSkip = 0);

        IEnumerable<IHumanoidTag> GetHumanoidTagsWithNoRefCount(int count, int limitSkip = 0);

        int CountHumanoidTagsForHumanoidTag(string name);

        void UpdateRefCount(IHumanoidTag humanoidTag);
    }
}

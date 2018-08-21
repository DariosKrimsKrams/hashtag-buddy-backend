namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IBlacklistStorage
    {
        void Insert(IEnumerable<string> entries);

        IEnumerable<IBlacklistEntry> GetAllBlacklistEntries();

        IEnumerable<IHumanoidTag> GetHumanoidTagsThatContain(string name);

        void UpdateHumanoidTags(IEnumerable<IHumanoidTag> hTags);
    }
}
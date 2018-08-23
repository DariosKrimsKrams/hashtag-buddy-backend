namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IBlacklistStorage
    {
        void Insert(IList<IBlacklistEntry> entries);

        void Delete(string reason, string table);

        IEnumerable<IBlacklistEntry> GetAllBlacklistEntries();

        IEnumerable<IHumanoidTag> GetHumanoidTagsThatContain(string name);

        IEnumerable<IMachineTag> GetMachineTagsThatContain(string name);

        void UpdateHumanoidTags(IEnumerable<IHumanoidTag> hTags);
    }
}
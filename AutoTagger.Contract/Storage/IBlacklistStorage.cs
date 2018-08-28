namespace AutoTagger.Contract.Models
{
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface IBlacklistStorage
    {
        void Insert(IList<IBlacklistEntry> entries);

        void Delete(string reason, string table);

        IEnumerable<IBlacklistEntry> GetAllBlacklistEntries();

        IEnumerable<IEntity> GetHumanoidTagsThatContain(string name);

        IEnumerable<IEntity> GetMachineTagsThatContain(string name);

        void UpdateTags(IEnumerable<IEntity> hTags, string table);
    }
}
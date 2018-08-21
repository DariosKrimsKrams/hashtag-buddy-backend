namespace AutoTagger.Contract.Storage
{
    using System.Collections.Generic;
    using AutoTagger.Contract.Models;

    public interface IDebugStorage
    {
        string GetPhotosCount();

        string GetHumanoidTagsCount();

        string GetHumanoidTagRelationCount();

        string GetMachineTagsCount();

        IEnumerable<ILog> GetLogs();
    }
}

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

        string GetLogCount();

        IEnumerable<ILog> GetLogs(int skip, int take, string orderby);

        ILog GetLog(int id);

        bool IsIdAndCustomerIdMatching(int id, string customerId);
    }
}

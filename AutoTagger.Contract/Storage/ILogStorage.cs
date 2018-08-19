namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface ILogStorage
    {
        int InsertLog(string data);

        void UpdateLog(ILog log);

        ILog GetLog(int id);
    }
}

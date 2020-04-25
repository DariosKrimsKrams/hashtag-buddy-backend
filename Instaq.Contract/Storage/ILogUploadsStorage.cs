namespace Instaq.Contract.Storage
{
    using Instaq.Contract.Models;

    public interface ILogUploadsStorage
    {
        int InsertLog(string data, string customerId);

        void UpdateLog(ILog log);

        ILog GetLog(int id);
    }
}

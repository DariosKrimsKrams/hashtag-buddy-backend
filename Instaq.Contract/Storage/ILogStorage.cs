namespace Instaq.Contract
{
    using Instaq.Contract.Models;

    public interface ILogStorage
    {
        int InsertLog(string data, string customerId);

        void UpdateLog(ILog log);

        ILog GetLog(int id);
    }
}

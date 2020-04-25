namespace Instaq.Contract.Storage
{
    public interface ILogHashtagSearchStorage
    {
        int InsertLog(string type, string data, string customerId);
    }
}

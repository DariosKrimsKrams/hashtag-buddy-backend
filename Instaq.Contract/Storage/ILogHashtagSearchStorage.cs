namespace Instaq.Contract
{
    public interface ILogHashtagSearchStorage
    {
        int InsertLog(string type, string data, string customerId);
    }
}

namespace Instaq.Contract.Storage
{
    public interface ILogSystem
    {
        void InsertLog(string logLevel, string data);
    }
}

namespace Instaq.API.Extern.Services.Interfaces
{
    using Instaq.API.Extern.Models.Dtos;

    public interface ILoggingService
    {
        //void LogRequest(ApiLogItem logItem);

        void LogInformation(ApiLogItem logItem);

        void LogInformation(string message);

        void LogDebug(string message);

        void LogFatal(string message);

        void LogFatal(string message, params object[] args);

        void LogError(string message);

        void LogWarning(string message);

    }
}

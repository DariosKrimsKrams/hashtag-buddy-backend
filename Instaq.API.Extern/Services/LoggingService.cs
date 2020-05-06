namespace Instaq.API.Extern.Services
{
    using System.Text.Json;
    using Instaq.API.Extern.Models.Dtos;
    using Instaq.API.Extern.Services.Interfaces;
    using Instaq.Contract.Storage;

    public class LoggingService : ILoggingService
    {
        private readonly ILogSystem logSystem;

        public LoggingService(ILogSystem logSystem)
        {
            this.logSystem = logSystem;
        }

        public void LogRequest(ApiLogItem logItem)
        {
            var data = JsonSerializer.Serialize(logItem);
            this.logSystem.InsertLog("request", data);
        }

    }
}

namespace Instaq.API.Extern.Services.Interfaces
{
    using Instaq.API.Extern.Models.Dtos;

    public interface ILoggingService
    {
        void Log(ApiLogItem logItem);
    }
}

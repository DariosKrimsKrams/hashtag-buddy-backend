namespace Instaq.API.Extern.Services
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.Json;
    using Instaq.API.Extern.Models.Dtos;
    using Instaq.API.Extern.Services.Interfaces;
    using Serilog;

    //public class LoggingService : ILoggingService
    //{
    //    private readonly ILogSystem logSystem;

    //    public LoggingService(ILogSystem logSystem)
    //    {
    //        this.logSystem = logSystem;
    //    }

    //    public void LogRequest(ApiLogItem logItem)
    //    {
    //        var data = JsonSerializer.Serialize(logItem);
    //        this.logSystem.InsertLog("request", data);
    //    }

    //}

    public class LoggingService : ILoggingService
    {
        //private readonly ILogger logger;

        private StackFrame GetStackFrame(int level = 4)
        {
            return new StackFrame(level, true);
        }

        private MethodBase? GetMethod()
        {
            var stackFrame = GetStackFrame(5);
            return stackFrame.GetMethod();
        }

        private string GetFileName()
        {
            var stackFrame = GetStackFrame();
            var fullPath = stackFrame.GetFileName();
            return Path.GetFileName(fullPath) ?? "Unknown";
        }

        private string GetClassName()
        {
            var method = GetMethod();
            return method != null && method.ReflectedType != null ? method.ReflectedType.Name : "unknown";
        }

        private string GetLineNumber()
        {
            return GetStackFrame().GetFileLineNumber().ToString();
        }

        private string GetCaller()
        {
            return $"{GetFileName()}:{GetClassName()}:{GetLineNumber()}";
        }

        public void LogDebug(string message)
        {
#if DEBUG
            Log.Logger.Debug($"{GetCaller()} - {message}");
#endif
        }

        public void LogInformation(ApiLogItem logItem)
        {
            var jsonString = JsonSerializer.Serialize(logItem);
            Log.Logger.Information($"{GetCaller()} - {jsonString}");
        }

        public void LogInformation(string message)
        {
            Log.Logger.Information($"{GetCaller()} - {message}");
        }

        public void LogWarning(string message)
        {
            Log.Logger.Warning($"{GetCaller()} - {message}");
        }

        public void LogError(string message)
        {
            Log.Logger.Error($"{GetCaller()} - {message}");
        }

        public void LogFatal(string message)
        {
            Log.Logger.Fatal($"{GetCaller()} - {message}");
        }

        public void LogFatal(string message, params object[] args)
        {
            Log.Logger.Fatal(message, args);
        }

    }
}

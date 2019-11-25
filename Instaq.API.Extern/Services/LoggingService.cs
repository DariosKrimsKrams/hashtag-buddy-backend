using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using API.Controllers;
//using API.Models.Dtos;
//using API.Services.Interfaces;
//using Log4NetLogger;
//using log4net.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using logger = Log4NetLogger.Log4NetLogger;

namespace API.Services
{
    using Instaq.API.Extern.Models.Dtos;
    using Instaq.API.Extern.Services.Interfaces;

    public class LoggingService : ILoggingService
    {
        //private readonly ILogger<LoggingService> logger;

        public LoggingService(IConfiguration configuration)
        {
            //var path = configuration.GetValue<string>("LogFile:Path");
            //logger = Log4NetLogger.CreateLogger<LoggingService>(path);
        }

        public void Log(ApiLogItem logItem)
        {
            //var data = logItem.ToJson();
            //logger.LogInformation(data);
        }
    }
}

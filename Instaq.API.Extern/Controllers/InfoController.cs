namespace Instaq.API.Extern.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Instaq.Common;
    using System;
    using Instaq.API.Extern.Helpers;

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class InfoController : ControllerBase
    {
        [HttpGet("Version")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Index()
        {
            var result = new Dictionary<string, string>
            {
                { "evaluationVersion", Config.Version.ToString() },
                { "date", Config.Date },
                { "ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "" },
                { "globalSettingEnv", GlobalSettings.Environment }
            };
            return this.Ok(result);
        }
    }
}

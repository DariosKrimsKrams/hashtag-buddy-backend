namespace Instaq.API.Extern.Controllers
{
    using System.Collections.Generic;
    using Instaq.API.Extern.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Instaq.Common;
    using Microsoft.Extensions.PlatformAbstractions;
    using System;

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class InfoController : ControllerBase
    {
        [HttpGet("Version")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Index()
        {
            var version = Config.Version;
            var date = Config.Date;

            var result = new Dictionary<string, string>
            {
                { "backendVersion", VersionInfo.Version },
                { "evaluationVersion", version.ToString() },
                { "date", date }
            };
            return this.Ok(result);
        }

        [HttpGet("Env")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Env()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return this.Ok(env);
        }
    }
}

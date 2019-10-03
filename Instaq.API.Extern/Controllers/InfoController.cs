namespace Instaq.API.Extern.Controllers
{
    using System.Collections.Generic;
    using Instaq.API.Extern.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Instaq.Common;

    [Route("[controller]")]
    [Produces("application/json")]
    public class InfoController : Controller
    {
        [HttpGet("Version")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Index()
        {
            var version = Config.Version;
            var date = Config.Date;

            var list = new Dictionary<string, string>
            {
                { "backendVersion", VersionInfo.Version },
                { "evaluationVersion", version.ToString() },
                { "date", date }
            };
            return this.Json(list);
        }
    }
}

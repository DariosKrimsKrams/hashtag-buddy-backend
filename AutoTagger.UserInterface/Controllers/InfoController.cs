using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagger.UserInterface.Controllers
{
    [Route("[controller]")]
    public class InfoController : Controller
    {
        [HttpGet("Version")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Index()
        {
            var version = "v0.2";
            var date = "2018-07-15";

            var list = new Dictionary<string, string>();
            list.Add("version", version);
            list.Add("date", date);
            return this.Json(list);
        }
    }
}

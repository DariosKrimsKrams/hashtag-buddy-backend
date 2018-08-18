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
            var version = "v0.3";
            var date = "2018-08-18";

            var list = new Dictionary<string, string>();
            list.Add("version", version);
            list.Add("date", date);
            return this.Json(list);
        }
    }
}

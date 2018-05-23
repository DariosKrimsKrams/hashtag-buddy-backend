using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTagger.UserInterface.Controllers
{
    public class InfoController : Controller
    {
        // GET: /<controller>/
        [HttpGet("Info/Version")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Index()
        {
            var version = "v0.1";

            var list = new Dictionary<string, string>();
            list.Add("version", version);
            return this.Json(list);
        }
    }
}

﻿namespace Instaq.API.Debug.Controllers
{
    using System.Collections.Generic;
    using Instaq.Common;
    using Microsoft.AspNetCore.Mvc;

    [Route( "[controller]" )]
    public class InfoController : Controller
    {
        [HttpGet( "Version" )]
        [ProducesResponseType( typeof( void ), 200 )]
        public IActionResult Index()
        {
            var version = Config.Version;
            var date    = Config.Date;

            var list = new Dictionary<string, string>();
            list.Add( "version", version.ToString() );
            list.Add( "date", date );
            return this.Json( list );
        }
    }
}

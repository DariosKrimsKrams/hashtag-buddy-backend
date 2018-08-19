using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagger.API.Controllers
{
    using AutoTagger.Contract.Storage;

    [Route("[controller]")]
    public class StatsController : Controller
    {
        private IDebugStorage debugStorage;

        public StatsController(IDebugStorage debugStorage)
        {
            this.debugStorage = debugStorage;
        }

        [Route("Photos")]
        [HttpGet]
        public IActionResult GetPhotosCount()
        {
            var count = this.debugStorage.GetPhotosCount();
            return this.Ok(count);
        }

        /*
         * 
// show photos count
SELECT count(*) from photos 

// show photos count
SELECT count(*) from itags 

// show photos count
SELECT count(*) from photo_itag_rel 

// count photos with mtags
SELECT count(distinct m.photoId) from mtags as m

         */

    }
}
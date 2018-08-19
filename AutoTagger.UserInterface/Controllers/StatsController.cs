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

        [Route("HumanoidTags")]
        [HttpGet]
        public IActionResult GetHumanoidTagsCount()
        {
            var count = this.debugStorage.GetHumanoidTagsCount();
            return this.Ok(count);
        }

        [Route("HumanoidTagRelations")]
        [HttpGet]
        public IActionResult GetHumanoidTagRelationCount()
        {
            var count = this.debugStorage.GetHumanoidTagRelationCount();
            return this.Ok(count);
        }

        [Route("MachineTags")]
        [HttpGet]
        public IActionResult GetMachineTagsCount()
        {
            var count = this.debugStorage.GetMachineTagsCount();
            return this.Ok(count);
        }

    }
}
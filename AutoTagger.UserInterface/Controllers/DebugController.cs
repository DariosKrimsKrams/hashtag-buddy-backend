using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagger.API.Controllers
{
    using AutoTagger.Contract.Models;
    using AutoTagger.Contract.Storage;

    [Route("[controller]")]
    public class DebugController : Controller
    {
        private IDebugStorage debugStorage;

        public DebugController(IDebugStorage debugStorage)
        {
            this.debugStorage = debugStorage;
        }

        [Route("Stats/PhotosCount")]
        [HttpGet]
        public IActionResult GetPhotosCount()
        {
            var count = this.debugStorage.GetPhotosCount();
            return this.Ok(count);
        }

        [Route("Stats/HumanoidTagsCount")]
        [HttpGet]
        public IActionResult GetHumanoidTagsCount()
        {
            var count = this.debugStorage.GetHumanoidTagsCount();
            return this.Ok(count);
        }

        [Route("Stats/HumanoidTagRelationsCount")]
        [HttpGet]
        public IActionResult GetHumanoidTagRelationCount()
        {
            var count = this.debugStorage.GetHumanoidTagRelationCount();
            return this.Ok(count);
        }

        [Route("Stats/MachineTagsCount")]
        [HttpGet]
        public IActionResult GetMachineTagsCount()
        {
            var count = this.debugStorage.GetMachineTagsCount();
            return this.Ok(count);
        }

        [Route("Logs/{page}")]
        [HttpGet]
        public IEnumerable<ILog> GetLogs(int page)
        {
            var count = 10;
            return this.debugStorage.GetLogs(count, count * (page - 1));
        }

    }
}
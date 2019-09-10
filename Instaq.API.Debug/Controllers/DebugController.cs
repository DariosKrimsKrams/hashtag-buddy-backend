namespace Instaq.API.Debug.Controllers
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route( "[controller]" )]
    public class DebugController : Controller
    {
        private IDebugStorage debugStorage;

        public DebugController( IDebugStorage debugStorage )
        {
            this.debugStorage = debugStorage;
        }

        [Route( "Stats/HumanoidTagRelationsCount" )]
        [HttpGet]
        public IActionResult GetHumanoidTagRelationCount()
        {
            var count = this.debugStorage.GetHumanoidTagRelationCount();
            return this.Ok( count );
        }

        [Route( "Stats/HumanoidTagsCount" )]
        [HttpGet]
        public IActionResult GetHumanoidTagsCount()
        {
            var count = this.debugStorage.GetHumanoidTagsCount();
            return this.Ok( count );
        }

        [Route( "Log/{id}" )]
        [HttpGet]
        public IActionResult GetLog( int id )
        {
            var log = this.debugStorage.GetLog( id );
            if( log == null )
            {
                return this.NotFound( "Log doesn't exist or was deleted :'(" );
            }

            var output = this.BuildLogOutput( log );
            return this.Ok( output );
        }

        [Route( "Logs/{skip}/{take}/{orderby}" )]
        [HttpGet]
        public IEnumerable<Dictionary<string, object>> GetLogs( int skip, int take, string orderby )
        {
            //var logsCount = this.debugStorage.GetLogCount();
            var logs = this.debugStorage.GetLogs( skip, take, orderby );
            //var items = new List<Dictionary<string, object>>();
            foreach( var log in logs )
            {
                yield return this.BuildLogOutput( log );
            }

            //var output = new Dictionary<string, object>();
            //output.Add("totalCount", logsCount);
            //output.Add("items", items);
            //return output;
        }

        [Route( "LogsCount" )]
        [HttpGet]
        public string GetLogsCount()
        {
            var logsCount = this.debugStorage.GetLogCount();
            return logsCount;
        }

        [Route( "Stats/MachineTagsCount" )]
        [HttpGet]
        public IActionResult GetMachineTagsCount()
        {
            var count = this.debugStorage.GetMachineTagsCount();
            return this.Ok( count );
        }

        [Route( "Stats/PhotosCount" )]
        [HttpGet]
        public IActionResult GetPhotosCount()
        {
            var count = this.debugStorage.GetPhotosCount();
            return this.Ok( count );
        }

        private Dictionary<string, object> BuildLogOutput( ILog log )
        {
            var entries = JsonConvert.DeserializeObject<Dictionary<string, object>>( log.Data );
            foreach( var entry in entries )
            {
                if( entry.Value is string valueAsStr && valueAsStr.Substring( 0, 2 ) == "[{" )
                {
                    entries[ entry.Key ] = JsonConvert.DeserializeObject<Dictionary<string, object>>( valueAsStr );
                }
            }

            entries.Add( "id", log.Id );
            entries.Add( "created", log.Created );
            return entries;
        }
    }
}

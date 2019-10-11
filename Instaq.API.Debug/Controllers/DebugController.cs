namespace Instaq.API.Debug.Controllers
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;

    [ApiController]
    [Route( "[controller]" )]
    public class DebugController : ControllerBase
    {
        private IDebugStorage debugStorage;

        public DebugController( IDebugStorage debugStorage )
        {
            this.debugStorage = debugStorage;
        }

        [HttpGet("Stats/HumanoidTagRelationsCount")]
        public IActionResult GetHumanoidTagRelationCount()
        {
            var count = this.debugStorage.GetHumanoidTagRelationCount();
            return this.Ok( count );
        }

        [HttpGet("Stats/HumanoidTagsCount")]
        public IActionResult GetHumanoidTagsCount()
        {
            var count = this.debugStorage.GetHumanoidTagsCount();
            return this.Ok( count );
        }

        [HttpGet("Log/{id}")]
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

        [HttpGet("Logs/{skip}/{take}/{orderby}")]
        public IEnumerable<Dictionary<string, object>> GetLogs( int skip, int take, string orderby )
        {
            var logs = this.debugStorage.GetLogs( skip, take, orderby );
            foreach( var log in logs )
            {
                yield return this.BuildLogOutput( log );
            }
        }

        [HttpGet("LogsCount")]
        public string GetLogsCount()
        {
            var logsCount = this.debugStorage.GetLogCount();
            return logsCount;
        }

        [HttpGet("Stats/MachineTagsCount")]
        public IActionResult GetMachineTagsCount()
        {
            var count = this.debugStorage.GetMachineTagsCount();
            return this.Ok( count );
        }

        [HttpGet("Stats/PhotosCount")]
        public IActionResult GetPhotosCount()
        {
            var count = this.debugStorage.GetPhotosCount();
            return this.Ok( count );
        }

        private Dictionary<string, object> BuildLogOutput( ILog log )
        {
            var entries = JsonSerializer.Deserialize<Dictionary<string, object>>(log.Data);
            foreach ( var entry in entries )
            {
                if( entry.Value is string valueAsStr && valueAsStr.Substring( 0, 2 ) == "[{" )
                {
                    entries[entry.Key] = JsonSerializer.Deserialize<Dictionary<string, object>>(valueAsStr);
                }
            }

            entries.Add( "id", log.Id );
            entries.Add( "created", log.Created );
            return entries;
        }
    }
}

namespace AutoTagger.API.Debug.Controllers
{
    using System;
    using System.Collections.Generic;

    using AutoTagger.API.Debug.Models;
    using AutoTagger.Contract;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IEvaluationStorage evaluationStorage;
        private readonly ILogStorage logStorage;
        private readonly IFileHandler fileHandler;

        public UserController(
            IEvaluationStorage evaluationStorage,
            ILogStorage logStorage,
            IFileHandler fileHandler
            )
        {
            this.evaluationStorage = evaluationStorage;
            this.logStorage = logStorage;
            this.fileHandler = fileHandler;
        }

        [HttpPost("App")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult Feedback([FromForm] FeedbackFormModel form)
        {
            //var key = "feedback";
            //try
            //{
            //    var log      = this.logStorage.GetLog(form.Id);
            //    var data     = log.GetDataAsList();
            //    var formData = JsonConvert.DeserializeObject<Dictionary<string, string>>(form.Data);
            //    if (data.ContainsKey(key))
            //    {
            //        data[key] = formData;
            //    }
            //    else
            //    {
            //        data.Add(key, formData);
            //    }
            //    log.SetData(data);

            //    this.logStorage.UpdateLog(log);
            //    return Ok();
            //}
            //catch (ArgumentException)
            //{
            //    return this.NotFound();
            //}
            return Ok();
        }

    }
}

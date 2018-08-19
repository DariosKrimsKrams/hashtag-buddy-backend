namespace AutoTagger.API.Controllers
{
    using System;
    using System.IO;

    using AutoTagger.API.Models;
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
            this.logStorage        = logStorage;
            this.fileHandler       = fileHandler;
        }

        [HttpPost("Feedback")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult SubmitFeedback([FromBody] FeedbackFormModel feedback)
        {
            var key = "feedback";
            try
            {
                var log = this.logStorage.GetLog(feedback.Id);
                var data = log.GetDataAsList();
                if (data.ContainsKey(key))
                {
                    data[key] = feedback.Data;
                }
                else
                {
                    data.Add(key, feedback.Data);
                }
                log.SetData(data);

                this.logStorage.UpdateLog(log);
                return Ok();
            }
            catch (ArgumentException)
            {
                return this.NotFound();
            }
        }

        [Route("Img/{fileName}")]
        [HttpGet]
        public IActionResult GetUserImage(string fileName)
        {
            if (fileName.Contains(".."))
            {
                return this.StatusCode(500);
            }
            try
            {
                var image = this.fileHandler.GetFile(FileType.User, fileName);
                return this.File(image, "image/jpeg");
            }
            catch (FileNotFoundException)
            {
                return this.NotFound("Image not found");
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }
    }
}

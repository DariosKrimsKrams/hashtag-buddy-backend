namespace AutoTagger.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using AutoTagger.API.Models;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("[controller]")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackStorage feedbackStorage;

        public FeedbackController(
            IFeedbackStorage feedbackStorage
            )
        {
            this.feedbackStorage = feedbackStorage;
        }

        [HttpPost("App")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult AppFeedback([FromForm] IFeedback feedback)
        {
            try
            {
                this.feedbackStorage.InsertLog(feedback);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.NotFound();
            }
        }

    }
}

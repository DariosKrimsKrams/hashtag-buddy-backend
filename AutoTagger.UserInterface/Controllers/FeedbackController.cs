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
        public IActionResult AppFeedback([FromForm] AppFeedbackFormModel feedback)
        {
            try
            {
                var data = "";
                this.HandleFeedback("app", feedback.CustomerId, data);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        [HttpPost("Results")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult ResultsFeedback([FromForm] ResultsFeedbackFormModel feedback)
        {
            try
            {
                var data = "";
                this.HandleFeedback("results", feedback.CustomerId, data);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        private void HandleFeedback(string type, string customerId, string data)
        {

            //this.feedbackStorage.InsertLog(appFeedback);
        }

    }
}

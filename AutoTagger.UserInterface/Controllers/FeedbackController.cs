namespace AutoTagger.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.API.Models;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("[controller]")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackStorage feedbackStorage;
        private readonly ICustomerStorage customerStorage;

        public FeedbackController(
            IFeedbackStorage feedbackStorage,
            ICustomerStorage customerStorage
            )
        {
            this.feedbackStorage = feedbackStorage;
            this.customerStorage = customerStorage;
        }

        [HttpPost("App")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult AppFeedback([FromBody] AppFeedbackFormModel feedback)
        {
            try
            {
                if (!this.IsCustomerValid(feedback.CustomerId))
                {
                    return this.Unauthorized();
                }

                var data = new Dictionary<string, string>();
                data.Add("email", feedback.Email);
                data.Add("message", feedback.Message);

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
        public IActionResult ResultsFeedback([FromBody] ResultsFeedbackFormModel feedback)
        {
            try
            {
                if (!this.IsCustomerValid(feedback.CustomerId))
                {
                    return this.Unauthorized();
                }
                
                var data = new Dictionary<string, string>();
                data.Add("photoId", feedback.PhotoId);
                data.Add("rating", feedback.Rating);
                data.Add("goodHashtags", feedback.GoodHashtags);
                data.Add("badHashtags", feedback.BadHashtags);
                data.Add("missingHashtags", feedback.MissingHashtags);
                data.Add("comment", feedback.Comment);

                this.HandleFeedback("results", feedback.CustomerId, data);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        private void HandleFeedback(string type, string customerId, Dictionary<string, string> data)
        {
            var json = JsonConvert.SerializeObject(data);


            var feedback = new Feedback
            {
                Type       = type,
                CustomerId = customerId,
                Data       = json
            };

            this.feedbackStorage.Insert(feedback);
        }

        private bool IsCustomerValid(string customerId)
        {
            return customerId.Length == 64 && this.customerStorage.Exists(customerId);
        }

    }
}

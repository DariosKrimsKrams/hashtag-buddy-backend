﻿namespace Instaq.API.Extern.Controllers
{
    using System;
    using System.Text.Json;
    using Instaq.API.Extern.Models.Requests;
    using Instaq.Common;
    using Instaq.Contract.Storage;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackStorage feedbackStorage;
        private readonly ICustomerStorage customerStorage;
        private readonly IDebugStorage debugStorage;

        public FeedbackController(
            IFeedbackStorage feedbackStorage,
            ICustomerStorage customerStorage,
            IDebugStorage debugStorage
            )
        {
            this.feedbackStorage = feedbackStorage;
            this.customerStorage = customerStorage;
            this.debugStorage = debugStorage;
        }

        [HttpPost("App")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult AppFeedback(AppFeedback feedback)
        {
            try
            {
                if (feedback.CustomerId is null)
                {
                    return this.NotFound();
                }
                if (!this.IsCustomerValid(feedback.CustomerId))
                {
                    return this.Unauthorized();
                }

                this.HandleFeedback("app", feedback.CustomerId, 0, feedback);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        [HttpPost("Results")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult ResultsFeedback(ResultsFeedback feedback)
        {
            try
            {
                if(feedback.CustomerId is null)
                {
                    return this.NotFound();
                }
                if (!this.IsCustomerValid(feedback.CustomerId))
                {
                    return this.Unauthorized();
                }
                if (!this.debugStorage.ArePhotoIdAndCustomerIdMatching(feedback.PhotoId, feedback.CustomerId))
                {
                    return this.Unauthorized();
                }
                this.HandleFeedback("results", feedback.CustomerId, feedback.PhotoId, feedback);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        private void HandleFeedback(string type, string customerId, int photoId, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var feedback = new Feedback
            {
                Type       = type,
                CustomerId = customerId,
                DebugId    = photoId,
                Data       = json
            };
            this.feedbackStorage.Insert(feedback);

            this.customerStorage.IncreaseFeedbackCount(customerId);
        }

        private bool IsCustomerValid(string customerId)
        {
            return customerId.Length == 64 && this.customerStorage.Exists(customerId);
        }

    }
}

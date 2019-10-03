namespace Instaq.API.Extern.Controllers
{
    using System;

    using Instaq.API.Extern.Models.Responses;
    using Instaq.API.Extern.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [Produces("application/json")]
    public class EvaluationController : Controller
    {
        private readonly IEvaluationService evaluationService;

        public EvaluationController(
            IEvaluationService evaluationService
            )
        {
            this.evaluationService = evaluationService;
        }

        [HttpPost("File/{customerId}")]
        [ProducesResponseType(typeof(EvaluateResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        public IActionResult File(IFormFile file, string customerId)
        {
            try
            {
                if (this.Request.ContentType == null || !this.Request.ContentType.Contains("multipart/form-data; boundary"))
                {
                    return this.BadRequest("Wrong ContentType :'(");
                }

                if (file == null || file.Length == 0)
                {
                    return this.BadRequest("No Files uploaded");
                }

                if (!this.evaluationService.IsCustomerValid(customerId))
                {
                    return this.Unauthorized();
                }

                var data  = this.evaluationService.EvaluateFile(customerId, file, this.Request);
                return this.Ok(data);
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }
    }
}

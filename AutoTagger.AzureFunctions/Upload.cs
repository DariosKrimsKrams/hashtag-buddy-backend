namespace AutoTagger.AzureFunctions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoTagger.Contract;
    using AutoTagger.Database.Standard.Storage.Mysql;
    using AutoTagger.Evaluation.Standard;
    using AutoTagger.ImageProcessor.Standard;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;

    public static class Upload
    {
        [FunctionName("Upload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            TraceWriter log)
        {
            if (req.ContentType == null || !req.ContentType.Contains("multipart/form-data; boundary"))
            {
                return new BadRequestObjectResult("Wrong ContentType");
            }

            var body  = req.Body;
            var files = req.Form.Files;
            if (body == null || body.Length == 0 || req.ContentLength == 0 || files.Count != 1)
            {
                return new BadRequestObjectResult("No File uploaded");
            }

            var         storage         = new MysqlUiStorage();
            var         taggingProvider = new GcpVision();
            IEvaluation evaluation      = new Evaluation();
            var debugInfos = new Dictionary<string, List<string>>
            {
                { "ip", new List<string> { req.HttpContext.Connection?.RemoteIpAddress?.ToString() } },
                { "backend_version", new List<string> { "0.2" } },
            };
            evaluation.AddDebugInfos(debugInfos);

            using (var stream = new MemoryStream())
            {
                await files[0].CopyToAsync(stream);
                var bytes = stream.ToArray();

                var machineTags = taggingProvider.GetTagsForImageBytes(bytes).ToList();

                if (!machineTags.Any())
                {
                    return new BadRequestObjectResult("No MachineTags found");
                }

                var mostRelevantHTags = evaluation.GetMostRelevantHumanoidTags(storage, machineTags);
                var trendingHTags     = evaluation.GetTrendingHumanoidTags(storage, machineTags, mostRelevantHTags);
                var result = new Dictionary<string, object>
                {
                    { "mostRelevantHTags", mostRelevantHTags },
                    { "trendingHTags", trendingHTags }
                };

                return new JsonResult(result);
            }
        }
    }
}

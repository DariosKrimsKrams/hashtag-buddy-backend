namespace AutoTagger.AzureFunctions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;

    using AutoTagger.Contract;
    using AutoTagger.Crawler.Standard;
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

            var storage         = new MysqlUIStorage();
            var taggingProvider = new GCPVision();
            IEvaluation evaluation = new Evaluation();
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

                //var machineTags = taggingProvider.GetTagsForImageBytes(bytes).ToList();

                // photo of Hamburg
                //var machineTags = new List<IMTag>
                //{
                //    new MTag("metropolitan area", 0.9740945f, "GCPVision_Label"),
                //    new MTag("urban area", 0.973557353f, "GCPVision_Label"),
                //    new MTag("city", 0.9734007f, "GCPVision_Label"),
                //    new MTag("cityscape", 0.938783467f, "GCPVision_Label"),
                //    new MTag("metropolis", 0.9214143f, "GCPVision_Label"),
                //    new MTag("landmark", 0.9191347f, "GCPVision_Label"),
                //    new MTag("sky", 0.916840732f, "GCPVision_Label"),
                //    new MTag("skyline", 0.905290544f, "GCPVision_Label"),
                //    new MTag("skyscraper", 0.884226561f, "GCPVision_Label"),
                //    new MTag("daytime", 0.851576f, "GCPVision_Label"),
                //    new MTag("Skyscraper", 0.9230419f, "GCPVision_Web"),
                //    new MTag("Metropolitan area", 0.7284566f, "GCPVision_Web"),
                //    new MTag("Bird's-eye view", 0.6971554f, "GCPVision_Web"),
                //    new MTag("Aerial photography", 0.659547f, "GCPVision_Web"),
                //    new MTag("Skyline", 0.6276192f, "GCPVision_Web"),
                //    new MTag("Tower", 0.627348959f, "GCPVision_Web"),
                //    new MTag("Cityscape", 0.5896153f, "GCPVision_Web"),
                //    new MTag("High-rise building", 0.56301415f, "GCPVision_Web"),
                //    new MTag("Urban area", 0.5345906f, "GCPVision_Web"),
                //    new MTag("Photography", 0.522f, "GCPVision_Web")
                //};

               // photo of Meat vs Vegan
               var machineTags = new List<IMTag>
               {
                    new MTag("fried food", 1.0f, "GCPVision_Label"),
                    new MTag("dish", 1.0f, "GCPVision_Label"),
                    new MTag("junk food", 1.0f, "GCPVision_Label"),
                    new MTag("kids meal", 1.0f, "GCPVision_Label"),
                    new MTag("food", 1.0f, "GCPVision_Label"),
                    new MTag("cuisine", 1.0f, "GCPVision_Label"),
                    new MTag("cuisine", 1.0f, "GCPVision_Label"),
                    new MTag("fast food", 1.0f, "GCPVision_Label"),
                    new MTag("side dish", 1.0f, "GCPVision_Label"),
                    new MTag("french fries", 1.0f, "GCPVision_Label"),
                    new MTag("french fries", 1.0f, "GCPVision_Label"),
                    new MTag("animal source foods", 1.0f, "GCPVision_Label"),
                    new MTag("French fries", 1.0f, "GCPVision_Web"),
                    new MTag("Full breakfast", 1.0f, "GCPVision_Web"),
                    new MTag("Fish and chips", 1.0f, "GCPVision_Web"),
                    new MTag("Chicken and chips", 1.0f, "GCPVision_Web"),
                    new MTag("Junk food", 1.0f, "GCPVision_Web"),
                    new MTag("German cuisine", 1.0f, "GCPVision_Web"),
                    new MTag("Breakfast", 1.0f, "GCPVision_Web"),
                    new MTag("Chicken as food", 1.0f, "GCPVision_Web"),
                    new MTag("Kids' meal", 1.0f, "GCPVision_Web"),
                    new MTag("Food", 1.0f, "GCPVision_Web"),
               };


                if (!machineTags.Any())
                {
                    return new BadRequestObjectResult("No MachineTags found");
                }

                var mostRelevantHTags = evaluation.GetMostRelevantHumanoidTags(storage, machineTags);
                var trendingHTags = evaluation.GetTrendingHumanoidTags(storage, machineTags, mostRelevantHTags);
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

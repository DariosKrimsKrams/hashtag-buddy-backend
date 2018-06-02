namespace AutoTagger.AzureFunctions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoTagger.Contract;
    using AutoTagger.Database.Standard.Storage.Mysql;
    using AutoTagger.ImageProcessor.Standard;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;

    //using Newtonsoft.Json;

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            TraceWriter log)
        {
            if (req.ContentType == null || !req.ContentType.Contains("multipart/form-data; boundary"))
            {
                return new BadRequestObjectResult("Wrong ContentType");
            }

            var body = req.Body;
            var files = req.Form.Files;
            if (body == null || body.Length == 0 || req.ContentLength == 0 || files.Count != 1)
            {
                return new BadRequestObjectResult("No File uploaded");
            }

            var storage = new MysqlUIStorage();
            var taggingProvider = new GCPVision();

            using (var stream = new MemoryStream())
            {
                await files[0].CopyToAsync(stream);
                var bytes = stream.ToArray();

                var machineTags = taggingProvider.GetTagsForImageBytes(bytes).ToList();

                if (!machineTags.Any())
                {
                    return new BadRequestObjectResult("No MachineTags found");
                }

                var content = FindTags(machineTags, req, storage);
                //var json    = JsonConvert.SerializeObject(content);
                var json = new JsonResult(content);

                var debug = content;
                //var debugStr = JsonConvert.SerializeObject(debug);
                //storage.Log("web_image", debugStr);

                return new JsonResult(content);
                //return (ActionResult)new OkObjectResult($"Hello, WORKS :D");
            }
        }

        private static Dictionary<string, object> FindTags(
            List<IMTag> machineTags,
            HttpRequest req,
            IAutoTaggerStorage storage)
        {
            var (query, instagramTags) = storage.FindHumanoidTags(machineTags);
            var ip = req.HttpContext.Connection?.RemoteIpAddress?.ToString();

            var data = new Dictionary<string, object>
            {
                { "machineTags", machineTags },
                { "instagramTags", instagramTags },
                { "query", query },
                { "ip", ip }
            };

            return data;
        }
    }
}

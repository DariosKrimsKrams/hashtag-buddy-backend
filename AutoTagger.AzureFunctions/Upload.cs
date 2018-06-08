namespace AutoTagger.AzureFunctions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;

    using AutoTagger.Contract;
    using AutoTagger.Database.Standard.Storage.Mysql;
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

            using (var stream = new MemoryStream())
            {
                await files[0].CopyToAsync(stream);
                var bytes = stream.ToArray();

                var machineTags = taggingProvider.GetTagsForImageBytes(bytes).ToList();

                if (!machineTags.Any())
                {
                    return new BadRequestObjectResult("No MachineTags found");
                }

                var (query, instagramTags) = storage.FindHumanoidTags(machineTags);
                var result = new Dictionary<string, object> { { "instagramTags", instagramTags } };

                SaveDebugInfos(req, machineTags, instagramTags, query, storage);

                return new JsonResult(result);
            }
        }

        private static void SaveDebugInfos(
            HttpRequest req,
            List<IMTag> machineTags,
            IEnumerable<string> instagramTags,
            string query,
            MysqlUIStorage storage)
        {
            var mTags = new List<string>();
            foreach (var mTag in machineTags)
            {
                mTags.Add($"{{\"Name\":\"{mTag.Name}\",\"Score\":{mTag.Score},\"Source\":\"{mTag.Source}\"}}");
            }

            var ip = req.HttpContext.Connection?.RemoteIpAddress?.ToString();
            var debugInfos = new Dictionary<string, List<string>>
            {
                { "machineTags", mTags },
                { "instagramTags", instagramTags.ToList() },
                { "query", new List<string> { query } },
                { "ip", new List<string> { ip } },
                { "backend_version", new List<string> { "0.2" } },
            };
            var json = SerializeJson(debugInfos);
            storage.Log("web_image", json);
        }

        private static string SerializeJson(Dictionary<string, List<string>> dict)
        {
            var stream = new MemoryStream();
            var ser    = new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>));
            ser.WriteObject(stream, dict);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}

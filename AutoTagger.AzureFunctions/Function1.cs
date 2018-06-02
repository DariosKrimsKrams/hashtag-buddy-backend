
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AutoTagger.AzureFunctions
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoTagger.ImageProcessor.Standard;

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {

            if (req.ContentType == null ||
                !req.ContentType.Contains("multipart/form-data; boundary"))
            {
                return new BadRequestObjectResult("Wrong ContentType");
            }

            var body = req.Body;
            var files = req.Form.Files;
            if (body == null || body.Length == 0 || req.ContentLength == 0 || files.Count != 1)
            {
                return new BadRequestObjectResult("No File uploaded");
            }

            //var storage = new MysqlUIStorage();
            //services.AddTransient<, MysqlUIStorage>();
            //services.AddTransient<ITaggingProvider, GCPVision>();
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

                //var content = this.FindTags(machineTags);
                //var json = this.Json(content);

                //var debug = content;
                //var debugStr = JsonConvert.SerializeObject(debug);
                //this.storage.Log("web_image", debugStr);

                //return json;
                return (ActionResult)new OkObjectResult($"Hello, WORKS :D");
            }
        }
    }
}

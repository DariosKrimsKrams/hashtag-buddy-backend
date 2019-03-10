namespace AutoTagger.UserInterface.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("[controller]")]
    public class EvaluationController : Controller
    {
        private readonly IEvaluationStorage evaluationStorage;
        private readonly ILogStorage logStorage;
        private readonly ITaggingProvider taggingProvider;
        private readonly IFileHandler fileHandler;
        private readonly IEvaluation evaluation;
        private readonly ICustomerStorage customerStorage;

        public EvaluationController(IEvaluationStorage evaluationStorage,
                                   ITaggingProvider taggingProvider,
                                   IFileHandler fileHandler,
                                   IEvaluation evaluation,
                                   ILogStorage logStorage,
                                   ICustomerStorage customerStorage
            )
        {
            this.evaluationStorage = evaluationStorage;
            this.logStorage      = logStorage;
            this.taggingProvider = taggingProvider;
            this.fileHandler     = fileHandler;
            this.evaluation      = evaluation;
            this.customerStorage = customerStorage;
        }

        [HttpPost("File/{customerId}")]
        [ProducesResponseType(typeof(void), 200)]
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

                if (!this.IsCustomerValid(customerId))
                {
                    return this.Unauthorized();
                }

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var bytes = stream.ToArray();

                    var machineTags = this.taggingProvider.GetTagsForImageBytes(bytes);

                    // photo of Hamburg
                    //var machineTags = new IMachineTag[]
                    //{
                    //    new MachineTag("metropolitan area", 0.9740945f, "GCPVision_Label"),
                    //    new MachineTag("urban area", 0.973557353f, "GCPVision_Label"),
                    //    new MachineTag("city", 0.9734007f, "GCPVision_Label"),
                    //    new MachineTag("cityscape", 0.938783467f, "GCPVision_Label"),
                    //    new MachineTag("metropolis", 0.9214143f, "GCPVision_Label"),
                    //    new MachineTag("landmark", 0.9191347f, "GCPVision_Label"),
                    //    new MachineTag("sky", 0.916840732f, "GCPVision_Label"),
                    //    new MachineTag("skyline", 0.905290544f, "GCPVision_Label"),
                    //    new MachineTag("skyscraper", 0.884226561f, "GCPVision_Label"),
                    //    new MachineTag("daytime", 0.851576f, "GCPVision_Label"),
                    //    new MachineTag("Skyscraper", 0.9230419f, "GCPVision_Web"),
                    //    new MachineTag("Metropolitan area", 0.7284566f, "GCPVision_Web"),
                    //    new MachineTag("Bird's-eye view", 0.6971554f, "GCPVision_Web"),
                    //    new MachineTag("Aerial photography", 0.659547f, "GCPVision_Web"),
                    //    new MachineTag("Skyline", 0.6276192f, "GCPVision_Web"),
                    //    new MachineTag("Tower", 0.627348959f, "GCPVision_Web"),
                    //    new MachineTag("Cityscape", 0.5896153f, "GCPVision_Web"),
                    //    new MachineTag("High-rise building", 0.56301415f, "GCPVision_Web"),
                    //    new MachineTag("Urban area", 0.5345906f, "GCPVision_Web"),
                    //    new MachineTag("Photography", 0.522f, "GCPVision_Web")
                    //};

                    // photo of Meat vs Vegan
                    //var machineTags = new List<IMachineTag>
                    //{
                    //     new MachineTag("fried food", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("dish", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("junk food", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("kids meal", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("food", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("cuisine", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("cuisine", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("fast food", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("side dish", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("french fries", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("french fries", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("animal source foods", 1.0f, "GCPVision_Label"),
                    //     new MachineTag("French fries", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Full breakfast", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Fish and chips", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Chicken and chips", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Junk food", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("German cuisine", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Breakfast", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Chicken as food", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Kids' meal", 1.0f, "GCPVision_Web"),
                    //     new MachineTag("Food", 1.0f, "GCPVision_Web"),
                    //};

                    if (machineTags.Length == 0)
                    {
                        return this.BadRequest("No MachineTags found :'(");
                    }

                    this.evaluation.AddDebugInfos("ip", this.GetIpAddress());
                    var data = this.FindTags(evaluation, machineTags);

                    var debugData = JsonConvert.SerializeObject(this.evaluation.GetDebugInfos());
                    var logId = this.logStorage.InsertLog(debugData, customerId);

                    var hash = GetHashString(logId.ToString());
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = hash + ext.ToLower();
                    this.fileHandler.Save(FileType.User, bytes, fileName);

                    this.evaluation.AddDebugInfos("image", fileName);
                    this.evaluation.AddDebugInfos("originalFilename", file.FileName);
                    debugData = JsonConvert.SerializeObject(this.evaluation.GetDebugInfos());
                    var log = new Log { Id = logId, Data = debugData };
                    this.logStorage.UpdateLog(log);

                    data.Add("img", fileName);
                    data.Add("logId", logId);
                    return this.Ok(data);
                }
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        public static string GetHashString(string input)
        {
            var sb = new StringBuilder();
            var algorithm = MD5.Create();
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));
            return sb.ToString().Substring(0, 10).ToLower();
        }

        private Dictionary<string, object> FindTags(IEvaluation evaluation, IMachineTag[] machineTags)
        {
            var mostRelevantHTags = evaluation.GetMostRelevantHumanoidTags(this.evaluationStorage, machineTags);
            var trendingHTags     = evaluation.GetTrendingHumanoidTags(this.evaluationStorage, machineTags, mostRelevantHTags);

            return new Dictionary<string, object>
            {
                { "mostRelevantHTags", mostRelevantHTags },
                { "trendingHTags", trendingHTags },
            };
        }

        private string GetIpAddress()
        {
            return this.Request.HttpContext.Connection?.RemoteIpAddress?.ToString();
        }

        private bool IsCustomerValid(string customerId)
        {
            return customerId.Length == 64 && this.customerStorage.Exists(customerId);
        }
    }
}

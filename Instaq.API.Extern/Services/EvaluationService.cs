namespace Instaq.API.Extern.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Instaq.API.Extern.Helpers;
    using Instaq.API.Extern.Models.Responses;
    using Instaq.API.Extern.Services.Interfaces;
    using Instaq.Common;
    using Instaq.Common.Utils;
    using Instaq.Contract;
    using Instaq.Contract.Dto;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Query;
    using Microsoft.AspNetCore.Http;

    using static System.String;

    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationStorage evaluationStorage;
        private readonly ILogUploadsStorage logUploadsStorage;
        private readonly ITaggingProvider taggingProvider;
        private readonly IFileHandler fileHandler;
        private readonly IEvaluation evaluation;
        private readonly ICustomerStorage customerStorage;
        private readonly ILogHashtagSearchStorage logHashtagSearchStorage;

        public EvaluationService(
            IEvaluationStorage evaluationStorage,
            ITaggingProvider taggingProvider,
            IFileHandler fileHandler,
            IEvaluation evaluation,
            ILogUploadsStorage logUploadsStorage,
            ICustomerStorage customerStorage,
            ILogHashtagSearchStorage logHashtagSearchStorage
        )
        {
            this.evaluationStorage = evaluationStorage;
            this.logUploadsStorage        = logUploadsStorage;
            this.taggingProvider   = taggingProvider;
            this.fileHandler       = fileHandler;
            this.evaluation        = evaluation;
            this.customerStorage   = customerStorage;
            this.logHashtagSearchStorage = logHashtagSearchStorage;
        }


        public bool IsCustomerValid(string customerId)
        {
            return customerId.Length == 64 && this.customerStorage.Exists(customerId);
        }

        public EvaluateResponse EvaluateFile(string customerId, IFormFile file, HttpRequest request)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            var bytes = stream.ToArray();

            IMachineTag[] machineTags;
            try
            {
                machineTags = this.taggingProvider.GetTagsForImageBytes(bytes);
            }
            catch (Exception e)
            {
                // ToDo write log with error into DB
                Console.WriteLine("Evaluation Error: " + e.Message);
                throw;
            }

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
                throw new Exception("No MachineTags found :'(");
            }

            this.evaluation.AddDebugInfos("ip", UserInfos.GetIpAddress(request));
            var response = this.FindTags(machineTags);

            var debugData = JsonSerializer.Serialize(this.evaluation.GetDebugInfos());
            var logId = this.logUploadsStorage.InsertLog(debugData, customerId);
            var hash = Hash.GetMd5(logId.ToString());
            var ext = Path.GetExtension(file.FileName);
            var fileName = hash + ext.ToLower();
            this.fileHandler.Save(FileType.User, bytes, fileName);

            this.evaluation.AddDebugInfos("image", fileName);
            this.evaluation.AddDebugInfos("originalFilename", file.FileName);
            debugData = JsonSerializer.Serialize(this.evaluation.GetDebugInfos());
            var log = new Log { Id = logId, Data = debugData };
            this.logUploadsStorage.UpdateLog(log);
            
            this.customerStorage.IncreasePhotosCount(customerId);

            response.Img = fileName;
            response.LogId = logId;

            return response;
        }

        private EvaluateResponse FindTags(IMachineTag[] machineTags)
        {
            var mostRelevantHTags = this.evaluation.GetMostRelevantHumanoidTags(this.evaluationStorage, machineTags);
            var trendingHTags     = this.evaluation.GetTrendingHumanoidTags(this.evaluationStorage, machineTags, mostRelevantHTags);

            var response = new EvaluateResponse
            {
                MostRelevantHTags = mostRelevantHTags,
                TrendingHTags = trendingHTags
            };

            return response;
        }

        public SearchResponse GetSimilarHashtags(string customerId, string keyword)
        {
            keyword = keyword.Trim().ToLower();
            if (IsNullOrEmpty(keyword))
            {
                throw new ArgumentException();
            }
            var machineTags = new IMachineTag[]
            {
                new MachineTag { Name = keyword }
            };
            var data = this.GetSimilarHashtags(customerId, machineTags, new List<string> { keyword }, "hashtag-search");
            this.customerStorage.IncreaseAmountOfHashtagSearchUsed(customerId);
            return data;
        }

        public SearchResponse GetSimilarHashtags(string customerId, IEnumerable<string> keywords, IEnumerable<string> excludeHashtags)
        {
            var machineTags = new List<IMachineTag>();
            var excludeHashtags2 = excludeHashtags.ToList();
            foreach (var keyword in keywords)
            {
                var keyword2 = keyword.Trim().ToLower();
                machineTags.Add(new MachineTag { Name = keyword2 });
                excludeHashtags2.Add(keyword2);
            }
            var machineTagsAsArray = machineTags.ToArray();
            return this.GetSimilarHashtags(customerId, machineTagsAsArray, excludeHashtags2, "image-suggestions");
        }

        private SearchResponse GetSimilarHashtags(string customerId, IMachineTag[] machineTags, IEnumerable<string> excludeHashtags, string type)
        {
            var response1 = this.evaluationStorage.FindHumanoidTags<FindSimilarMachineTagsQuery>(machineTags);
            var response2 = this.evaluationStorage.FindHumanoidTags<FindSimilarToHumanoidTagsQuery>(machineTags);

            var humanoidTags = MergeHashtags(response1, response2);
            ExcludeHashtags(excludeHashtags, humanoidTags);
            humanoidTags = SortHashtags(humanoidTags);
            this.LogSearch(customerId, machineTags, type, response1, response2);

            return new SearchResponse
            {
                LogId    = 0,
                Hashtags = humanoidTags
            };
        }

        private static List<IHumanoidTag> MergeHashtags(IEvaluationDto response1, IEvaluationDto response2)
        {
            var humanoidTags = response1.HumanoidTags.ToList();
            foreach (var htag in response2.HumanoidTags)
            {
                var exist = humanoidTags.FirstOrDefault(x => x.Name == htag.Name);
                if (exist == null)
                {
                    humanoidTags.Add(htag);
                }
            }

            return humanoidTags;
        }

        private static void ExcludeHashtags(IEnumerable<string> excludeHashtags, List<IHumanoidTag> humanoidTags)
        {
            for (var i = humanoidTags.Count - 1; i >= 0; i--)
            {
                var hashtag = humanoidTags[i];
                if (excludeHashtags.Contains(hashtag.Name))
                {
                    humanoidTags.RemoveAt(i);
                }
            }
        }

        private List<IHumanoidTag> SortHashtags(List<IHumanoidTag> humanoidTags)
        {
            var results = new List<IHumanoidTag>();
            var results2 = new List<IHumanoidTag>();

            foreach (var humanoidTag in humanoidTags)
            {
                if (humanoidTag.Name.Length >= 10)
                {
                    results.Add(humanoidTag);
                }
                else
                {
                    results2.Add(humanoidTag);
                }
            }

            return results.Concat(results2).ToList();
        }

        private void LogSearch(
            string customerId,
            IMachineTag[] machineTags,
            string type,
            IEvaluationDto response1,
            IEvaluationDto response2)
        {
            var data = new Dictionary<string, object>();
            data.Add("input", JsonSerializer.Serialize(machineTags));
            data.Add("result1 FindSimilarMachineTagsQuery", response1);
            data.Add("result2 FindSimilarToHumanoidTagsQuery", response2);
            var dataAsJson = JsonSerializer.Serialize(data);
            this.logHashtagSearchStorage.InsertLog(type, dataAsJson, customerId);
        }

    }
}

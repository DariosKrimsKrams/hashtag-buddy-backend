using System;

namespace AutoTagger.Evaluation.Standard
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;

    using AutoTagger.Contract;
    using AutoTagger.Evaluation.Standard.PostProcessor;

    public class Evaluation : IEvaluation
    {
        public IEnumerable<string> GetHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMTag> mTags)
        {
            var (query, instagramTags) = storage.FindHumanoidTags(mTags);
            var result = new Dictionary<string, object> { { "instagramTags", instagramTags } };
            //SaveDebugInfos(req, machineTags, instagramTags, query, storage);

            instagramTags = new OrderByAmountOfPosts().Do(instagramTags);

            var iTags = new List<string>();
            foreach (var instagramTag in instagramTags)
            {
                iTags.Add(instagramTag.FirstOrDefault());
            }

            return iTags;
        }


        //private static void SaveDebugInfos(
        //    HttpRequest req,
        //    List<IMTag> machineTags,
        //    IEnumerable<string> instagramTags,
        //    string query,
        //    IAutoTaggerStorage storage)
        //{
        //    var mTags = new List<string>();
        //    foreach (var mTag in machineTags)
        //    {
        //        mTags.Add($"{{\"Name\":\"{mTag.Name}\",\"Score\":{mTag.Score},\"Source\":\"{mTag.Source}\"}}");
        //    }

        //    var ip = req.HttpContext.Connection?.RemoteIpAddress?.ToString();
        //    var debugInfos = new Dictionary<string, List<string>>
        //    {
        //        { "machineTags", mTags },
        //        { "instagramTags", instagramTags.ToList() },
        //        { "query", new List<string> { query } },
        //        { "ip", new List<string> { ip } },
        //        { "backend_version", new List<string> { "0.2" } },
        //    };
        //    var json = SerializeJson(debugInfos);
        //    storage.Log("web_image", json);
        //}

        //private static string SerializeJson(Dictionary<string, List<string>> dict)
        //{
        //    var stream = new MemoryStream();
        //    var ser    = new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>));
        //    ser.WriteObject(stream, dict);
        //    stream.Position = 0;
        //    var sr = new StreamReader(stream);
        //    return sr.ReadToEnd();
        //}

    }
}

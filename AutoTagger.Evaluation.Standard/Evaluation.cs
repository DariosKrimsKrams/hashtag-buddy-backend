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
        private Dictionary<string, IEnumerable<string>> debugInfos;

        public Evaluation()
        {
            this.debugInfos = new Dictionary<string, IEnumerable<string>>();
        }

        public void AddDebugInfos(Dictionary<string, List<string>> moreDebugInfos)
        {
            moreDebugInfos.ToList().ForEach(x => this.debugInfos.Add(x.Key, x.Value));
        }

        public IEnumerable<string> GetHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMTag> mTags)
        {
            var (query, hTags) = storage.FindHumanoidTags(mTags);

            hTags = new OrderByAmountOfPosts().Do(hTags);

            var iTags = new List<string>();
            foreach (var instagramTag in hTags)
            {
                iTags.Add(instagramTag.FirstOrDefault());
            }

            SaveDebugInfos(mTags, iTags, query, storage);

            return iTags;
        }


        private void SaveDebugInfos(
            IEnumerable<IMTag> machineTags,
            IEnumerable<string> instagramTags,
            string query,
            IAutoTaggerStorage storage)
        {
            var mTags = new List<string>();
            foreach (var mTag in machineTags)
            {
                mTags.Add($"{{\"Name\":\"{mTag.Name}\",\"Score\":{mTag.Score},\"Source\":\"{mTag.Source}\"}}");
            }

            this.debugInfos.Add("machineTags", mTags);
            this.debugInfos.Add("instagramTags", instagramTags);
            this.debugInfos.Add("query", new List<string> { query });

            var json = SerializeJson(this.debugInfos);
            storage.Log("web_image", json);
        }

        private static string SerializeJson(Dictionary<string, IEnumerable<string>> dict)
        {
            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>));
            ser.WriteObject(stream, dict);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

    }
}

namespace AutoTagger.Evaluation.Standard
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using AutoTagger.Contract;

    public class Evaluation : IEvaluation
    {
        private Dictionary<string, List<string>> debugInfos;

        public Evaluation()
        {
            this.debugInfos = new Dictionary<string, List<string>>();
        }

        public void AddDebugInfos(Dictionary<string, List<string>> moreDebugInfos)
        {
            moreDebugInfos.ToList().ForEach(x => this.debugInfos.Add(x.Key, x.Value));
        }

        public IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMachineTag> mTags)
        {
            var (query, hTags) = storage.FindMostRelevantHumanoidTags(mTags);

            SaveDebugInfos(mTags, hTags, query, storage);

            //hTags = new OrderByAmountOfPosts().Do(hTags);

            return hTags;
        }

        public IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMachineTag> mTags, IEnumerable<IHumanoidTag> mostRelevantHTags)
        {
            var (queryTrending, hTagsTrending) = storage.FindTrendingHumanoidTags(mTags);
            var hTagsTrendingList = hTagsTrending.ToList();

            for (var i = hTagsTrendingList.Count - 1; i >= 0; i--)
            {
                var htagTrending = hTagsTrendingList[i];
                var exists       = mostRelevantHTags.FirstOrDefault(x => x.Name == htagTrending.Name);
                if (exists != null)
                    hTagsTrendingList.RemoveAt(i);
            }

            //SaveDebugInfos(mTags, hTags, query, storage);

            return hTagsTrendingList;
        }

        private void SaveDebugInfos(
            IEnumerable<IMachineTag> machineTags,
            IEnumerable<IHumanoidTag> instagramTags,
            string query,
            IAutoTaggerStorage storage)
        {
            var mTags = new List<string>();
            foreach (var mTag in machineTags)
            {
                mTags.Add($"{{\"Name\":\"{mTag.Name}\",\"Score\":{mTag.Score},\"Source\":\"{mTag.Source}\"}}");
            }

            var iTags = new List<string>();
            foreach (var instagramTag in instagramTags)
            {
                var str = "";
                var instagramTag2 = new List<string>()
                {
                    instagramTag.Name,
                    instagramTag.Posts.ToString()
                };
                for (var i = 0; i < instagramTag2.Count; i++)
                {
                    var val = instagramTag2[i];
                    str += $"\"{i}\":\"{val}\",";
                }
                iTags.Add($"{{{str.TrimEnd(',')}}}");
            }

            this.debugInfos.Add("machineTags", mTags.ToList());
            this.debugInfos.Add("instagramTags", iTags.ToList());
            this.debugInfos.Add("query", new List<string> { query });

            var json = SerializeJson(this.debugInfos);
            storage.Log("web_image", json);
        }

        private static string SerializeJson(Dictionary<string, List<string>> dict)
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

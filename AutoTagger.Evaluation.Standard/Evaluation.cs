namespace AutoTagger.Evaluation.Standard
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using AutoTagger.Contract;

    using Newtonsoft.Json;

    public class Evaluation : IEvaluation
    {
        private Dictionary<string, object> debugInfos;

        public Evaluation()
        {
            this.debugInfos = new Dictionary<string, object>();

        }

        public void AddDebugInfos(Dictionary<string, List<string>> moreDebugInfos)
        {
            moreDebugInfos.ToList().ForEach(x => this.debugInfos.Add(x.Key, x.Value));
        }

        public IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(IUiStorage storage, IEnumerable<IMachineTag> machineTags)
        {
            var (query, humanoidTags) = storage.FindMostRelevantHumanoidTags(machineTags);

            this.debugInfos.Add("machineTags", machineTags);
            this.debugInfos.Add("humanoidTagsMostRelevant", humanoidTags);
            this.debugInfos.Add("queryMostRelevant", query);
            SaveDebugInfos(storage);

            //hTags = new OrderByAmountOfPosts().Do(hTags);

            return humanoidTags;
        }

        public IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(IUiStorage storage, IEnumerable<IMachineTag> mTags, IEnumerable<IHumanoidTag> mostRelevantHTags)
        {
            var (query, humanoidTags) = storage.FindTrendingHumanoidTags(mTags);
            var hTagsTrendingList = humanoidTags.ToList();

            for (var i = hTagsTrendingList.Count - 1; i >= 0; i--)
            {
                var htagTrending = hTagsTrendingList[i];
                var exists       = mostRelevantHTags.FirstOrDefault(x => x.Name == htagTrending.Name);
                if (exists != null)
                    hTagsTrendingList.RemoveAt(i);
            }

            this.debugInfos.Add("humanoidTagsTrending", hTagsTrendingList);
            this.debugInfos.Add("queryTrending", query);

            return hTagsTrendingList;
        }

        private void SaveDebugInfos(IUiStorage storage)
        {
            var json = JsonConvert.SerializeObject(this.debugInfos);
            storage.Log("web_image", json);
        }
    }
}

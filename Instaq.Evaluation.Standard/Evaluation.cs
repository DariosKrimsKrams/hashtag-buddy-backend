namespace Instaq.Evaluation.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Contract.Models;

    public class Evaluation : IEvaluation
    {
        private readonly Dictionary<string, object> debugInfos;

        public Evaluation()
        {
            this.debugInfos = new Dictionary<string, object>();
        }

        public void AddDebugInfos(string key, object value)
        {
            this.debugInfos.Add(key, value);
        }

        public Dictionary<string, object> GetDebugInfos()
        {
            return this.debugInfos;
        }

        public IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(
            IEvaluationStorage storage,
            IMachineTag[] machineTags)
        {
            var (query, humanoidTags) = storage.FindMostRelevantHumanoidTags(machineTags);

            this.debugInfos.Add("machineTags", machineTags);
            this.debugInfos.Add("backend_version", Config.Version);
            this.debugInfos.Add("backend_date", Config.Date);
            this.debugInfos.Add("humanoidTagsMostRelevant", humanoidTags);
            this.debugInfos.Add("queryMostRelevant", query);

            // Post Processor Example
            //humanoidTags = new OrderByAmountOfPosts().Do(humanoidTags);

            return humanoidTags;
        }

        public IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(
            IEvaluationStorage storage,
            IMachineTag[] machineTags,
            IEnumerable<IHumanoidTag> mostRelevantHTags)
        {
            var (query, humanoidTags) = storage.FindTrendingHumanoidTags(machineTags);
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
    }
}

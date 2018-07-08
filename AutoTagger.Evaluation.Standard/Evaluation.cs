﻿namespace AutoTagger.Evaluation.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using Newtonsoft.Json;

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

        public IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(
            IUiStorage storage,
            IEnumerable<IMachineTag> machineTags)
        {
            var (query, humanoidTags) = storage.FindMostRelevantHumanoidTags(machineTags);

            this.debugInfos.Add("machineTags", machineTags);
            this.AddDebugInfos("backend_version", "0.2");
            this.debugInfos.Add("humanoidTagsMostRelevant", humanoidTags);
            this.debugInfos.Add("queryMostRelevant", query);
            this.SaveDebugInfos(storage);

            //hTags = new OrderByAmountOfPosts().Do(hTags);

            return humanoidTags;
        }

        public IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(
            IUiStorage storage,
            IEnumerable<IMachineTag> mTags,
            IEnumerable<IHumanoidTag> mostRelevantHTags)
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
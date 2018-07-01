using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface IEvaluation
    {
        IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMachineTag> mTags);

        IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMachineTag> mTags, IEnumerable<IHumanoidTag> mostRelevantHTags);

        void AddDebugInfos(Dictionary<string, List<string>> moreDebugInfos);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IEvaluation
    {
        void AddDebugInfos(Dictionary<string, List<string>> moreDebugInfos);

        IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(IUiStorage storage, IEnumerable<IMachineTag> mTags);

        IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(
            IUiStorage storage,
            IEnumerable<IMachineTag> mTags,
            IEnumerable<IHumanoidTag> mostRelevantHTags);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IEvaluation
    {
        void AddDebugInfos(string key, object value);

        Dictionary<string, object> GetDebugInfos();

        IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(IEvaluationStorage storage, IEnumerable<IMachineTag> mTags);

        IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(
            IEvaluationStorage storage,
            IEnumerable<IMachineTag> mTags,
            IEnumerable<IHumanoidTag> mostRelevantHTags);
    }
}

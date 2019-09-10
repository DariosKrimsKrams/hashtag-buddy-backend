namespace Instaq.Contract
{
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface IEvaluation
    {
        void AddDebugInfos(string key, object value);

        Dictionary<string, object> GetDebugInfos();

        IEnumerable<IHumanoidTag> GetMostRelevantHumanoidTags(
            IEvaluationStorage storage,
            IMachineTag[] machineTags);

        IEnumerable<IHumanoidTag> GetTrendingHumanoidTags(
            IEvaluationStorage storage,
            IMachineTag[] mTags,
            IEnumerable<IHumanoidTag> mostRelevantHTags);
    }
}

namespace Instaq.Contract
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public interface IEvaluationStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IMachineTag[] mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IMachineTag[] mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags<T>(IMachineTag[] machineTags)
            where T : IFindHumanoidTagsQuery;

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}

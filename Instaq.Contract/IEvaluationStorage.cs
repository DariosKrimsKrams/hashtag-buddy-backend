namespace Instaq.Contract
{
    using System.Collections.Generic;

    using Instaq.Contract.Dto;
    using Instaq.Contract.Models;

    public interface IEvaluationStorage
    {
        IEvaluationDto FindMostRelevantHumanoidTags(IMachineTag[] mTags);

        IEvaluationDto FindTrendingHumanoidTags(IMachineTag[] mTags);

        IEvaluationDto FindHumanoidTags<T>(IMachineTag[] machineTags)
            where T : IFindHumanoidTagsQuery;

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}

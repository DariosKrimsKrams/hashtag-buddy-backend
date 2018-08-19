namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IEvaluationStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IEnumerable<IMachineTag> mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> mTags);

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}

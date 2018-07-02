namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IAutoTaggerStorage
    {
        void Log(string source, string data);

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();

        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IEnumerable<IMachineTag> mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> mTags);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IUiStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IEnumerable<IMachineTag> mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> mTags);

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();

        void Log(string source, string data);
    }
}

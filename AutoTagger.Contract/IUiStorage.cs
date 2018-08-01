namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IUiStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IEnumerable<IMachineTag> mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> mTags);

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();

        int InsertLog(string data);

        void UpdateLog(int id, string data);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IAutoTaggerStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags(IEnumerable<IMTag> machineTags);
        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMTag> machineTags);
        void Log(string source, string data);
        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IAutoTaggerStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags(IEnumerable<IMachineTag> machineTags);
        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> machineTags);
        void Log(string source, string data);
        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}

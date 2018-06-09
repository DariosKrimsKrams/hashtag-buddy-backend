namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IAutoTaggerStorage
    {
        (string debug, IEnumerable<string> htags) FindHumanoidTags(IEnumerable<IMTag> machineTags);
        void Log(string source, string data);
        IEnumerable<string> GetMtagsWithHighScore();
    }
}

namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawler
    {
        IEnumerable<IImage> DoCrawling(int limit, params string[] customTags);
        event Action<IHumanoidTag> OnHashtagFound;
        bool OverrideCondition(string key, int value);
        int GetCondition(string key);
        void BuildTags(string[] customTags);
    }
}

namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawler
    {
        event Action<IHumanoidTag> OnHashtagFound;

        event Action<IImage> OnImageFound;

        //void BuildTags(string[] customTags);

        void DoCrawling(int limit, params string[] customTags);

        void SetSetting(string key, int value);

    }
}

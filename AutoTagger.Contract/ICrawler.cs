namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawler
    {
        event Action<IHumanoidTag> OnHashtagFoundComplete;

        event Action<IEnumerable<string>> OnHashtagNamesFound;

        event Action<IImage> OnImageFound;

        void DoCrawling(params string[] customTags);

        void UpdateSettings(ICrawlerSettings settings);
    }
}

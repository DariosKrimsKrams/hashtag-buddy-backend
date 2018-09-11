namespace AutoTagger.Contract
{
    using System;

    public interface ICrawler
    {
        event Action<IHumanoidTag> OnHashtagFoundComplete;

        event Action<string> OnHashtagNameFound;

        event Action<IImage> OnImageFound;

        void DoCrawling(params string[] customTags);
    }
}

namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawlerStorage
    {
        void InsertImage(IImage image);

        void InsertHumanoidTags(IHumanoidTag[] hTags);

        List<TimeSpan> GetTimings(string type);
    }
}

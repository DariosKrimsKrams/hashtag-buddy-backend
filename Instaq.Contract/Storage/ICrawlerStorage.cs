namespace Instaq.Contract
{
    using System;
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface ICrawlerStorage
    {
        void InsertImages(IImage[] image);

        void InsertHumanoidTags(IHumanoidTag[] hTags);

        IEnumerable<TimeSpan> GetTimings(string type);
    }
}

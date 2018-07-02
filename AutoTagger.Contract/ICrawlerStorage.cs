namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawlerStorage
    {
        void Upsert(IImage image);
        IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag;
        void InsertOrUpdateHumanoidTag(IHumanoidTag hTag);
    }
}

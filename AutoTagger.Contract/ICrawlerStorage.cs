namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface ICrawlerStorage
    {
        void InsertOrUpdate(IImage image);
        IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag;
        void InsertOrUpdateHumaniodTag(IHumanoidTag hTag);
    }
}

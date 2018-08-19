namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface ICrawlerStorage
    {
        IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag;

        void InsertOrUpdateHumanoidTag(IHumanoidTag hTag);

        void Upsert(IImage image);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface ICrawlerStorage
    {
        IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag;

        void UpsertHumanoidTag(IHumanoidTag hTag);

        void Upsert(IImage image);

        void Save();
    }
}

namespace AutoTagger.Contract
{
    public interface ICrawlerStorage
    {
        void InsertImage(IImage image);

        void InsertHumanoidTags(IHumanoidTag[] hTags);
    }
}

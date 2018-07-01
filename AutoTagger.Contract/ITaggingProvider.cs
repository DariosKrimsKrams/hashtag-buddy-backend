namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface ITaggingProvider
    {
        IEnumerable<IMachineTag> GetTagsForFile(string path);

        IEnumerable<IMachineTag> GetTagsForImageBytes(byte[] bytes);

        IEnumerable<IMachineTag> GetTagsForImageUrl(string imageUrl);
    }
}

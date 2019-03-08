namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface ITaggingProvider
    {
        IEnumerable<IMachineTag> GetTagsForFile(string path);

        IMachineTag[] GetTagsForImageBytes(byte[] bytes);

        IEnumerable<IMachineTag> GetTagsForImageUrl(string imageUrl);
    }
}

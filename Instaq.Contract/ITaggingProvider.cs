namespace Instaq.Contract
{
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface ITaggingProvider
    {
        IEnumerable<IMachineTag> GetTagsForFile(string path);

        IMachineTag[] GetTagsForImageBytes(byte[] bytes);

        IEnumerable<IMachineTag> GetTagsForImageUrl(string imageUrl);
    }
}

namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    public interface IImageProcessorStorage
    {
        void Save();

        IEnumerable<IImage> GetImagesWithoutMachineTags(DateTime created, int limit);

        IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes);

        DateTime GetCreatedDateForLatestPhotoWithMTags();

        void InsertMachineTagsWithoutSaving(IImage image);
    }
}

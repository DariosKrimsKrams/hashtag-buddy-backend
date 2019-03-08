namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface IImageProcessorStorage
    {
        void Save();

        IEnumerable<IImage> GetImagesForImageDownloader(int limit);

        void SetImagesStatus(IEnumerable<string> shortcodes, string status);

        IEnumerable<IImage> GetImagesForCv();

        DateTime GetCreatedDateForLatestPhotoWithMTags();

        void InsertMachineTagsWithoutSaving(IImage image);
    }
}

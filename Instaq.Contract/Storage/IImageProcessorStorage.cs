namespace Instaq.Contract.Storage
{
    using System;
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface IImageProcessorStorage
    {
        void Save();

        IEnumerable<IImage> GetImagesWithEmptyStatus(int limit);

        void SetImagesStatus(IEnumerable<string> shortcodes, string status);

        IEnumerable<IImage> GetImagesForCv();

        DateTime GetCreatedDateForLatestPhotoWithMTags();

        void InsertMachineTagsWithoutSaving(IImage image);
    }
}

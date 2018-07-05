namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IImageProcessorStorage
    {
        void Save();

        IEnumerable<IImage> GetImagesWithoutMachineTags(int idLargerThan, int limit);

        IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes);

        int GetLargestPhotoIdForPhotoWithMTag();

        void InsertMachineTagsWithoutSaving(IImage image);
    }
}

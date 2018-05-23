using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface IImageProcessorStorage
    {

        IEnumerable<IImage> GetImagesWithoutMachineTags(int idLargerThan, int limit);
        IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes);
        void InsertMachineTagsWithoutSaving(IImage image);
        void DoSave();
        int GetLargestPhotoIdForPhotoWithMTag();
    }
}

namespace AutoTagger.Test.Core
{
    using AutoTagger.Common;
    using AutoTagger.Database.Storage.Mysql;
    using AutoTagger.ImageProcessor.Standard;

    using Xunit;

    public class ImageProcessing
    {
        [Fact]
        public void GcpVisionFromUri()
        {
            var tagger = new GcpVision();
            var uri = "https://www.bilderdepot24.de/item/images/1149747/1000x1000/1149747_1.jpg";
            var mtags = tagger.GetTagsForImageUrl(uri);
            
            var db = new MysqlImageProcessorStorage();
            var image = new Image { MachineTags = mtags, Id = 1 };
            db.InsertMachineTagsWithoutSaving(image);
            db.Save();
        }
    }
}

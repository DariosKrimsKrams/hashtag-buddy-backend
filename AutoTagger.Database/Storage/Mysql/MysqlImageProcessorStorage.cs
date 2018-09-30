namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Database;

    public class MysqlImageProcessorStorage : MysqlBaseStorage, IImageProcessorStorage
    {
        public new void Save()
        {
            base.Save();
        }

        public IEnumerable<IImage> GetImagesWithoutMachineTags(int idLargerThan, int limit)
        {
            //var query = (from p in this.db.Photos where p.Mtags.Count == 0 && p.Id > idLargerThan select p).Take(limit)
            //    .OrderBy(x => x.Id);
            //return query.ToList().Select(x => x.ToImage());
            return null;
        }

        public IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes)
        {
            var query = this.db.Photos.Where(p => shortCodes.Contains(p.Shortcode) && p.Mtags.Count == 0);
            return query.ToList().Select(x => x.ToImage());
        }

        public int GetLargestPhotoIdForPhotoWithMTag()
        {
            //return this.db.Mtags.Max(m => m.PhotoId);
            return 0;
        }

        public void InsertMachineTagsWithoutSaving(IImage image)
        {
            foreach (var mTag in image.MachineTags)
            {
                var mTagDb = new Mtags {
                    Name = mTag.Name,
                    Score = mTag.Score,
                    Source = mTag.Source,
                    PhotoId = image.Shortcode
                };
                this.db.Mtags.Add(mTagDb);
            }
        }
    }
}

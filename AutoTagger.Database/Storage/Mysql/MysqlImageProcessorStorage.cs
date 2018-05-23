namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::AutoTagger.Contract;
    using global::AutoTagger.Database.Mysql;

    public class MysqlImageProcessorStorage : MysqlBaseStorage, IImageProcessorStorage
    {
        private readonly Random random;

        public MysqlImageProcessorStorage()
        {
            this.random = new Random();
        }

        public IEnumerable<IImage> GetImagesWithoutMachineTags(int idLargerThan, int limit)
        {
            var query = (from p in this.db.Photos
                         where p.Mtags.Count == 0
                            && p.Id > idLargerThan
                         select p).Take(limit);
            return query.ToList().Select(x => x.ToImage());
        }

        public IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes)
        {
            var query = this.db.Photos.Where(p => shortCodes.Contains(p.Shortcode) && p.Mtags.Count == 0);
            return query.ToList().Select(x => x.ToImage());
        }

        public void InsertMachineTagsWithoutSaving(IImage image)
        {
            foreach (var mTag in image.MachineTags)
            {
                this.db.Mtags.Add(new Mtags {
                    Name = mTag.Name,
                    Score = mTag.Score,
                    Source = mTag.Source,
                    PhotoId = image.Id

                });
            }
        }

        public void DoSave()
        {
            this.Save();
        }

        public int GetLargestPhotoIdForPhotoWithMTag()
        {
            return this.db.Mtags.Max(m => m.PhotoId);
        }
    }
}

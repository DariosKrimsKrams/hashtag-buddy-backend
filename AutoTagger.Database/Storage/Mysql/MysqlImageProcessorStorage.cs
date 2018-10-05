namespace AutoTagger.Database.Storage.Mysql
{
    using System;
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

        public IEnumerable<IImage> GetImagesWithoutMachineTags(DateTime created, int limit)
        {
            var query = (from p in this.db.Photos where p.Mtags.Count == 0 && p.Created > created select p).Take(limit)
                .OrderBy(x => x.Created);
            var list = query.ToList();
            return list.Select(x => x.ToImage());
        }

        public IEnumerable<IImage> GetImagesWithoutMachineTags(IEnumerable<string> shortCodes)
        {
            var query = this.db.Photos.Where(p => shortCodes.Contains(p.Shortcode) && p.Mtags.Count == 0);
            return query.ToList().Select(x => x.ToImage());
        }

        public DateTime GetCreatedDateForLatestPhotoWithMTags()
        {
            var query = "select created from photos where shortcode = ( "
                     + "select shortcode from mtags order by id desc limit 1 )";
            var (results, _) = this.ExecuteCustomQuery(query);
            var date = results?.FirstOrDefault()?.FirstOrDefault();
            return Convert.ToDateTime(date);
        }

        public void InsertMachineTagsWithoutSaving(IImage image)
        {
            foreach (var mTag in image.MachineTags)
            {
                var mTagDb = new Mtags {
                    Name = mTag.Name,
                    Score = mTag.Score,
                    Source = mTag.Source,
                    Shortcode = image.Shortcode
                };
                this.db.Mtags.Add(mTagDb);
            }
        }
    }
}

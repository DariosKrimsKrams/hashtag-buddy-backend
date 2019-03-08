namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Database;

    public class MysqlImageProcessorStorage : MysqlBaseStorage, IImageProcessorStorage
    {
        public new void Save()
        {
            base.Save();
        }

        public IEnumerable<IImage> GetImagesForImageDownloader(int limit)
        {
            var query = this.db.Photos
                .Where(p => string.IsNullOrEmpty(p.Status))
                .OrderBy(x => x.Created).Take(limit);
            var list = query.ToList();
            return list.Select(x => x.ToImage());
        }

        public void SetImagesStatus(IEnumerable<string> shortcodes, string status)
        {
            var where = "";
            foreach (var shortcode in shortcodes)
            {
                where += $"`shortcode` = '{shortcode}' OR ";
            }
            char[] charsToTrim = { ' ', 'O', 'R' };
            where = where.TrimEnd(charsToTrim);
            var query = $"update photos set status = '{status}' where {where}";
            this.ExecuteCustomQuery(query);
        }

        public IEnumerable<IImage> GetImagesForCv()
        {
            var query = this.db.Photos
                .Where(p => p.Status == "readyForCv");
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

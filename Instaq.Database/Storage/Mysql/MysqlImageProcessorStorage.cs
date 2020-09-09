namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;
    using MySql.Data.MySqlClient;

    public class MysqlImageProcessorStorage : MysqlBaseStorage, IImageProcessorStorage
    {

        public MysqlImageProcessorStorage(InstaqContext context)
            : base(context)
        {
        }

        public new void Save()
        {
            base.Save();
        }

        public IEnumerable<IImage> GetImagesWithEmptyStatus(int limit)
        {
            try
            {
                return this.GetImagesWithEmptyStatusExecution(limit);
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Cannot Open when State is Connecting"))
                {
                    Console.WriteLine("Had problem with 'Cannot Open when State is Connecting' -> Retry...");
                    Thread.Sleep(3000);
                    return this.GetImagesWithEmptyStatusExecution(limit);
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private IEnumerable<IImage> GetImagesWithEmptyStatusExecution(int limit)
        {
            var query = this.Db.Photos
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
            var query = this.Db.Photos
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
                this.Db.Mtags.Add(mTagDb);
            }
        }
    }
}

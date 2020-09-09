namespace Instaq.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlDebugStorage : MysqlBaseStorage, IDebugStorage
    {
        public MysqlDebugStorage(InstaqContext context)
            : base(context)
        {
        }

        public bool ArePhotoIdAndCustomerIdMatching(int photoId, string customerId)
        {
            return this.Db.LogsUpload.FirstOrDefault(x => x.Id == photoId && x.CustomerId == customerId && x.Deleted == false)
                != null;
        }

        public string GetHumanoidTagRelationCount()
        {
            var query = "SELECT count(*) from photo_itag_rel";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

        public string GetHumanoidTagsCount()
        {
            var query = "SELECT count(*) FROM itags";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

        public ILog GetLog(int id)
        {
            return this.Db.LogsUpload.FirstOrDefault(x => x.Id == id && x.Deleted == false)?.ToLog();
        }

        public string GetLogCount()
        {
            var query = "SELECT count(*) from debug";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

        public IEnumerable<ILog> GetLogs(int skip, int take, string orderby)
        {
            return this.Db.LogsUpload.Where(x => x.Deleted == false).OrderByDescending(x => x.Id).Skip(skip).Take(take)
                .Select(x => x.ToLog());
        }

        public string GetMachineTagsCount()
        {
            var query = "SELECT count(distinct m.shortcode) from mtags as m";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

        public string GetPhotosCount()
        {
            var query = "SELECT count(*) as count from photos";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }
    }
}

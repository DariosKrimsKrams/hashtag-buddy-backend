namespace Instaq.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;

    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlDebugStorage : MysqlBaseStorage, IDebugStorage
    {
        public MysqlDebugStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public bool ArePhotoIdAndCustomerIdMatching(int photoId, string customerId)
        {
            return this.db.Debug.FirstOrDefault(x => x.Id == photoId && x.CustomerId == customerId && x.Deleted == 0)
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
            return this.db.Debug.FirstOrDefault(x => x.Id == id && x.Deleted == 0)?.ToLog();
        }

        public string GetLogCount()
        {
            var query = "SELECT count(*) from debug";
            var (results, time) = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

        public IEnumerable<ILog> GetLogs(int skip, int take, string orderby)
        {
            return this.db.Debug.Where(x => x.Deleted == 0).OrderByDescending(x => x.Id).Skip(skip).Take(take)
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

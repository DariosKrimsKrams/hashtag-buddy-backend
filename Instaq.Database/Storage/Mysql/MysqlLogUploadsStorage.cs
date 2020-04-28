namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Linq;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlLogUploadsStorage : MysqlBaseStorage, ILogUploadsStorage
    {
        public MysqlLogUploadsStorage(InstaqContext context)
            : base(context)
        {
        }

        public ILog GetLog(int id)
        {
            var entry = this.Db.LogsUpload.FirstOrDefault(x => x.Id == id);
            if (entry is null)
            {
                throw new ArgumentException();
            }
            return entry.ToLog();
        }

        public int InsertLog(string data, string customerId)
        {
            var debug = new LogsUpload { Data = data, CustomerId = customerId };
            this.Db.LogsUpload.Add(debug);
            this.Db.SaveChanges();
            return debug.Id;
        }

        public void UpdateLog(ILog log)
        {
            var existingEntry = this.Db.LogsUpload.First(x => x.Id == log.Id);
            existingEntry.Data = log.Data;
            this.Db.SaveChanges();
        }
    }
}

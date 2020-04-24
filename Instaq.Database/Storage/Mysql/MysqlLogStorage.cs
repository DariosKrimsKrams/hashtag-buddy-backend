namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Linq;

    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlLogStorage : MysqlBaseStorage, ILogStorage
    {
        public MysqlLogStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public ILog GetLog(int id)
        {
            var entry = this.Db.Debug.FirstOrDefault(x => x.Id == id);
            if (entry is null)
            {
                throw new ArgumentException();
            }

            return entry.ToLog();
        }

        public int InsertLog(string data, string customerId)
        {
            var debug = new Debug { Data = data, CustomerId = customerId };
            this.Db.Debug.Add(debug);
            this.Db.SaveChanges();
            return debug.Id;
        }

        public void UpdateLog(ILog log)
        {
            var existingEntry = this.Db.Debug.First(x => x.Id == log.Id);
            existingEntry.Data = log.Data;
            this.Db.SaveChanges();
        }
    }
}

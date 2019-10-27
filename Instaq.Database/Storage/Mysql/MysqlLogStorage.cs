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
            var entry = this.db.Debug.FirstOrDefault(x => x.Id == id);
            if (entry is null)
            {
                throw new ArgumentException();
            }

            return entry.ToLog();
        }

        public int InsertLog(string data, string customerId)
        {
            var debug = new Debug { Data = data, CustomerId = customerId };
            this.db.Debug.Add(debug);
            this.db.SaveChanges();
            return debug.Id;
        }

        public void UpdateLog(ILog log)
        {
            var existingEntry = this.db.Debug.First(x => x.Id == log.Id);
            existingEntry.Data = log.Data;
            this.db.SaveChanges();
        }
    }
}

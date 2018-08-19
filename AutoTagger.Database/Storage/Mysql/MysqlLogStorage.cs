namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Database.Storage.Mysql.Generated;

    public class MysqlLogStorage : MysqlBaseStorage, ILogStorage
    {
        public int InsertLog(string data)
        {
            var debug = new Debug { Data = data };
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

        public ILog GetLog(int id)
        {
            var entry = this.db.Debug.FirstOrDefault(x => x.Id == id);
            if (entry == null)
            {
                throw new ArgumentException();
            }
            return entry.toLog();
        }
    }
}

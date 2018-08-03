namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Database.Storage.Mysql.Generated;

    public class MysqlBlacklistStorage : MysqlBaseStorage, IBlacklistStorage
    {
        public void Insert(IEnumerable<string> entries)
        {
            foreach (var entry in entries)
            {
                var item = new Blacklist { Name = entry, Reason = "location" };
                this.db.Blacklist.Add(item);
            }
            this.db.SaveChanges();
        }
    }
}

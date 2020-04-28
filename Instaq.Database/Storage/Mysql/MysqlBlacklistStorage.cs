namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlBlacklistStorage : MysqlBaseStorage, IBlacklistStorage
    {

        public MysqlBlacklistStorage(InstaqContext context)
            : base(context)
        {
        }

        public void Insert(IList<IBlacklistEntry> entries)
        {
            foreach (var entry in entries)
            {
                var item = Blacklist.FromBlacklistEntry(entry);
                var query = "INSERT IGNORE INTO blacklist(`name`, `reason`, `table`)"
                          + $" VALUES ('{item.Name}', '{item.Reason}', '{item.Table}')";
                this.ExecuteCustomQuery(query);
            }
        }

        public void Delete(string reason, string table)
        {
            var entries = this.Db.Blacklist.Where(x => x.Reason == reason && x.Table == table);
            foreach (var entry in entries)
            {
                this.Db.Blacklist.Remove(entry);
            }
            this.Db.SaveChanges();
        }

        public (IEnumerable<ITag> tags, TimeSpan time) GetTags(string tableName="itags", int limit=1000)
        {
            var query = $"select i.name from {tableName} as i join blacklist as b on i.name LIKE concat('%', b.name, '%') where i.onBlacklist = 0 and b.table = 'itags' LIMIT {limit}";
            return this.ExecuteHTagsQuery(query);
        }

        public void UpdateTags(ITag[] tags, string table)
        {
            if (!tags.Any())
            {
                return;
            }
            var names = "";
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(names))
                    names += " OR ";
                names += "`name`='" + tag.Name + "'";
            }
            var query = $"UPDATE {table} SET `onBlacklist` = '1' WHERE {names}";
            this.ExecuteCustomQuery(query);
        }
    }
}

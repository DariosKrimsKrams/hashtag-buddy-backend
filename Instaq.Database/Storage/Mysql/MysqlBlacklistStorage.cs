﻿namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Database;

    public class MysqlBlacklistStorage : MysqlBaseStorage, IBlacklistStorage
    {
        public void Insert(IList<IBlacklistEntry> entries)
        {
            foreach (var entry in entries)
            {
                var item = Blacklist.FromBlacklistEntry(entry);
                var query = "INSERT IGNORE INTO blacklist(`name`, `reason`, `table`)"
                          + $" VALUES ('{entry.Name}', '{entry.Reason}', '{entry.Table}')";
                this.ExecuteCustomQuery(query);
            }
        }

        public void Delete(string reason, string table)
        {
            var entries = this.db.Blacklist.Where(x => x.Reason == reason && x.Table == table);
            foreach (var entry in entries)
            {
                this.db.Blacklist.Remove(entry);
            }
            this.db.SaveChanges();
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

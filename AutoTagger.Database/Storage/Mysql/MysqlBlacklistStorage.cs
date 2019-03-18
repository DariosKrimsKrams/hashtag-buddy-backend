namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Database;

    public class MysqlBlacklistStorage : MysqlBaseStorage, IBlacklistStorage
    {
        public void Insert(IList<IBlacklistEntry> entries)
        {
            foreach (var entry in entries)
            {
                var item = Blacklist.FromBlacklistEntry(entry);
                this.db.Blacklist.Add(item);
            }
            this.db.SaveChanges();
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

        public IEnumerable<IBlacklistEntry> GetAllBlacklistEntries()
        {
            foreach (var blacklist in this.db.Blacklist)
            {
                yield return blacklist.ToBlacklistEntry();
            }
        }

        public IEnumerable<IEntity> GetHumanoidTagsThatContain(string name)
        {
            return this.db.Itags
                .Where(i => i.Name.Contains(name) && i.OnBlacklist == 0)
                .Select(x => x.ToHumanoidTag());
        }

        public IEnumerable<IEntity> GetMachineTagsThatContain(string name)
        {
            return this.db.Mtags
                .Where(m => m.Name.Contains(name) && m.OnBlacklist == 0)
                .Select(x => x.ToMachineTag());
        }

        public void UpdateTags(IEnumerable<string> tags, string table)
        {
            var enumerable = tags as string[] ?? tags.ToArray();
            if (!enumerable.Any())
            {
                return;
            }
            var names = "";
            foreach (var tag in enumerable)
            {
                if (!string.IsNullOrEmpty(names))
                    names += " OR ";
                names += "`name`='" + tag + "'";
            }
            var query = $"UPDATE {table} SET `onBlacklist` = '1' WHERE {names}";
            this.ExecuteCustomQuery(query);
        }
    }
}

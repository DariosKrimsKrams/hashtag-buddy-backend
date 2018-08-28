namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;
    using AutoTagger.Database.Storage.Mysql.Generated;

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

        public void UpdateTags(IEnumerable<IEntity> tags, string table)
        {
            if (!tags.Any())
            {
                return;
            }
            var ids = "";
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += " OR ";
                ids += "id=" + tag.Id;
            }
            var query = $"UPDATE {table} SET `onBlacklist` = '1' WHERE {ids}";
            this.ExecuteCustomQuery(query);
        }
    }
}

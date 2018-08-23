namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    using AutoTagger.Common;
    using AutoTagger.Contract;
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
            var results = new List<IBlacklistEntry>();
            foreach (var blacklist in this.db.Blacklist)
            {
                results.Add(blacklist.ToBlacklistEntry());
            }
            return results;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTagsThatContain(string name)
        {
            return this.db.Itags
                .Where(i => i.Name.Contains(name) && i.OnBlacklist == 0)
                .Select(x => x.ToHumanoidTag());
        }

        public IEnumerable<IMachineTag> GetMachineTagsThatContain(string name)
        {
            return this.db.Mtags
                .Where(m => m.Name.Contains(name) && m.OnBlacklist == 0)
                .Select(x => x.ToHumanoidTag());
        }

        public void UpdateHumanoidTags(IEnumerable<IHumanoidTag> hTags)
        {
            if (!hTags.Any())
            {
                return;
            }
            var ids = "";
            foreach (var hTag in hTags)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += " OR ";
                ids += "id=" + hTag.Id;
            }
            var query = "UPDATE itags set onBlacklist = '1' where " + ids;
            this.ExecuteCustomQuery(query);
        }

    }
}

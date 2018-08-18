namespace AutoTagger.Database.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Common;
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

        public IEnumerable<IBlacklistEntryDto> GetAllBlacklistEntries()
        {
            var results = new List<IBlacklistEntryDto>();
            foreach (var blacklistEntry in db.Blacklist)
            {
                results.Add(new BlacklistEntryDto
                {
                    Id = blacklistEntry.Id,
                    Name = blacklistEntry.Name,
                    Reason = blacklistEntry.Reason
                });
            }
            return results;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTagsThatContain(string name)
        {
            return this.db.Itags
                .Where(i => i.Name.Contains(name) && i.OnBlacklist == 0)
                .Select(x => x.ToHumanoidTag());
        }

        public void UpdateHumanoidTags(IEnumerable<IHumanoidTag> hTags)
        {
            var ids = "";
            foreach (var hTag in hTags)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += " OR ";
                ids += "id=" + hTag.Id;
            }
            var query = "UPDATE itags set onBlacklist = 1 where " + ids;
            this.ExecuteCustomQuery(query);
        }

    }
}

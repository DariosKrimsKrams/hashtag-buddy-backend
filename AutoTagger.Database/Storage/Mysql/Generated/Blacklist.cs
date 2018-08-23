namespace AutoTagger.Database.Storage.Mysql.Generated
{
    using AutoTagger.Common;
    using AutoTagger.Contract;

    public partial class Blacklist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public string Table { get; set; }

        public static Blacklist FromBlacklistEntry(IBlacklistEntry entry)
        {
            return new Blacklist
            {
                Name = entry.Name,
                Reason = entry.Reason,
                Table = entry.Table
            };
        }

        public IBlacklistEntry ToBlacklistEntry()
        {
            return new BlacklistEntry
            {
                Id     = this.Id,
                Name   = this.Name,
                Reason = this.Reason,
                Table  = this.Table
            };
        }
    }
}

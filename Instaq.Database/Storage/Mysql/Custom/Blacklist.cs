namespace Instaq.Database
{
    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Contract.Models;

    public partial class Blacklist
    {
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

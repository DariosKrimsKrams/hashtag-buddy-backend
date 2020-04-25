namespace Instaq.Database.Storage.Mysql.Generated
{
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
    }
}

namespace AutoTagger.Common
{
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public class BlacklistEntry : IBlacklistEntry
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Reason { get; set; }

        public string Table { get; set; }
    }
}

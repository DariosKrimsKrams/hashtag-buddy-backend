namespace AutoTagger.Common
{
    using AutoTagger.Contract;

    public class BlacklistEntryDto : IBlacklistEntryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Reason { get; set; }
    }
}

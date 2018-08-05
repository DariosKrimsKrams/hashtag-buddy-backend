namespace AutoTagger.Contract
{
    public interface IBlacklistEntryDto
    {
        int Id { get; set; }

        string Name { get; set; }

        string Reason { get; set; }
    }
}

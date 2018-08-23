namespace AutoTagger.Contract
{
    public interface IBlacklistEntry
    {
        int Id { get; set; }

        string Name { get; set; }

        string Reason { get; set; }

        string Table { get; set; }
    }
}

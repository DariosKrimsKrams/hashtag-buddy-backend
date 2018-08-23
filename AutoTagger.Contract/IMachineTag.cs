namespace AutoTagger.Contract
{
    public interface IMachineTag
    {
        int Id { get; set; }

        string Name { get; set; }

        float Score { get; set; }

        string Source { get; set; }

        sbyte OnBlacklist { get; set; }
    }
}

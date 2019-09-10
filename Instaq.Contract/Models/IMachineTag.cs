namespace AutoTagger.Contract.Models
{
    public interface IMachineTag : ITag
    {
        string Name { get; set; }

        float Score { get; set; }

        string Source { get; set; }

        bool OnBlacklist { get; set; }
    }
}

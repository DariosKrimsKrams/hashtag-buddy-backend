namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface IMachineTag : IEntity
    {
        string Name { get; set; }

        float Score { get; set; }

        string Source { get; set; }

        bool OnBlacklist { get; set; }
    }
}

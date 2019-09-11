namespace Instaq.Contract.Models
{
    public interface IMachineTag : ITag
    {
        float Score { get; set; }

        string Source { get; set; }

        bool OnBlacklist { get; set; }
    }
}

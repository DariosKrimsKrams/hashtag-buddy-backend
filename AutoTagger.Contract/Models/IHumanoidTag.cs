namespace AutoTagger.Contract.Models
{
    public interface IHumanoidTag : ITag
    {
        string Name { get; set; }

        int Posts { get; set; }

        int RefCount { get; set; }

        bool OnBlacklist { get; set; }
    }
}

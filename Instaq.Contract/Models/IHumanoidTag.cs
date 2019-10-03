namespace Instaq.Contract.Models
{
    public interface IHumanoidTag : ITag
    {
        int Posts { get; set; }

        int RefCount { get; set; }

        bool OnBlacklist { get; set; }

        new string Name { get; set; }
    }
}

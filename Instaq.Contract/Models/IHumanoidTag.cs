namespace Instaq.Contract.Models
{
    public interface IHumanoidTag : ITag
    {
        int Posts { get; set; }

        int RefCount { get; set; }

        bool OnBlacklist { get; set; }

        // https: //github.com/aspnet/AspNetCore/issues/14724
        new string Name { get; set; }
    }
}

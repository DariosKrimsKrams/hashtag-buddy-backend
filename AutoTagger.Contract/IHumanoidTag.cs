namespace AutoTagger.Contract
{
    public interface IHumanoidTag
    {
        int Id { get; set; }

        string Name { get; set; }

        int Posts { get; set; }

        int RefCount { get; set; }
    }
}

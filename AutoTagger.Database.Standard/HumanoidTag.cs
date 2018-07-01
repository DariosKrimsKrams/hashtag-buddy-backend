namespace AutoTagger.Database.Standard
{
    using AutoTagger.Contract;

    public class HumanoidTag : IHumanoidTag
    {
        public string Name { get; set; }
        public int Posts { get; set; }
    }
}

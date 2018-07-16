namespace AutoTagger.Common
{
    using AutoTagger.Contract;

    public class HumanoidTag : IHumanoidTag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Posts { get; set; }

        public int RefCount { get; set; }
    }
}

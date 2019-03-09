namespace AutoTagger.Common
{
    using AutoTagger.Contract.Models;

    public class Feedback : IFeedback
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string UserId { get; set; }

        public string Data { get; set; }
    }
}

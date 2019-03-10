namespace AutoTagger.Contract.Models
{
    public interface IFeedback
    {
        int Id { get; set; }

        string Type { get; set; }

        string CustomerId { get; set; }

        string Data { get; set; }
    }
}

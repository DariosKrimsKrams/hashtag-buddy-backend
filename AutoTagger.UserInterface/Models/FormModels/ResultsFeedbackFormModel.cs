namespace AutoTagger.API.Models
{
    public class ResultsFeedbackFormModel
    {
        public string CustomerId { get; set; }

        public string PhotoId { get; set; }
        
        public string Rating { get; set; }

        public string GoodHashtags { get; set; }

        public string BadHashtags { get; set; }

        public string MissingHashtags { get; set; }

        public string Comment { get; set; }
    }
}

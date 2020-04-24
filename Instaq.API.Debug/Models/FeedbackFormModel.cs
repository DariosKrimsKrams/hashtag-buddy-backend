namespace Instaq.API.Debug.Models
{
    public class FeedbackFormModel
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public FeedbackFormModel()
        {
            this.Data = "";
        }
    }
}

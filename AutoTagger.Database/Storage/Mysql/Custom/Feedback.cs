namespace AutoTagger.Database
{
    using AutoTagger.Contract.Models;

    public partial class Feedback
    {
        public static Feedback FromCommonFeedback(IFeedback feedback)
        {
            return new Feedback
            {
                Id         = feedback.Id,
                Type       = feedback.Type,
                CustomerId = feedback.CustomerId,
                Data       = feedback.Data
            };
        }
    }
}

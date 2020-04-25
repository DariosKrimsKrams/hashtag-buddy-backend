namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Contract.Models;

    public partial class LogsFeedback
    {
        public static LogsFeedback FromCommonFeedback(IFeedback feedback)
        {
            return new LogsFeedback
            {
                Id         = feedback.Id,
                Type       = feedback.Type,
                CustomerId = feedback.CustomerId,
                DebugId    =  feedback.DebugId,
                Data       = feedback.Data
            };
        }
    }
}

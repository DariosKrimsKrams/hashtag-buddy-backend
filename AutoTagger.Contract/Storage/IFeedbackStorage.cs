namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface IFeedbackStorage
    {
        int InsertLog(IFeedback feedback);

        IFeedback GetLog(int id);
    }
}

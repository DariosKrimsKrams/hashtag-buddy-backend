namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface IFeedbackStorage
    {
        int Insert(IFeedback feedback);

        IFeedback GetLog(int id);
    }
}

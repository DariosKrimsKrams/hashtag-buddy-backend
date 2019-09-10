namespace Instaq.Contract
{
    using Instaq.Contract.Models;

    public interface IFeedbackStorage
    {
        int Insert(IFeedback feedback);

        IFeedback GetLog(int id);
    }
}

namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface ICustomerStorage
    {
        int Create(ICustomer customer);

        void IncreasePhotosCount(string customerId);

        void IncreaseFeedbackCount(string customerId);

        void UpdateCustomerId(int id, string customerId);

        bool Exists(string customerId);

        ICustomer Get(int id);
    }
}

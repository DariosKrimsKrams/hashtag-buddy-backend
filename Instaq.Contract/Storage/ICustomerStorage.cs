namespace Instaq.Contract
{
    using Instaq.Contract.Models;

    public interface ICustomerStorage
    {
        int Create(ICustomer customer);

        void IncreasePhotosCount(string customerId);

        void IncreaseFeedbackCount(string customerId);

        void UpdateCustomerId(int id, string customerId);

        void UpdateInfos(string customerId, string data);

        bool Exists(string customerId);

        ICustomer Get(int id);
    }
}

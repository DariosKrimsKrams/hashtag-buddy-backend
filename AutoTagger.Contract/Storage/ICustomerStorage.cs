namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface ICustomerStorage
    {
        int Create(ICustomer customer);

        void Update(ICustomer customer);

        void UpdateCustomerId(int id, string customerId);

        bool Exists(string customerId);

        ICustomer Get(int id);
    }
}

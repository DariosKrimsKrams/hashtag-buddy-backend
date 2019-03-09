namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface ICustomerStorage
    {
        int Create(ICustomer customer);

        void Update(ICustomer customer);
    }
}

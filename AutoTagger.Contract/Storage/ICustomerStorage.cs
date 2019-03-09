namespace AutoTagger.Contract
{
    using AutoTagger.Contract.Models;

    public interface ICustomerStorage
    {
        ICustomer Create(ICustomer customer);

        void Update(ICustomer customer);
    }
}

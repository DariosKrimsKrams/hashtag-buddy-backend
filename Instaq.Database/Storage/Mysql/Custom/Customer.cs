namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Contract.Models;

    public partial class Customer : IIdentifier
    {
        public static Customer FromCommonCustomer(ICustomer customer)
        {
            return new Customer
            {
                Id            = customer.Id,
                CustomerId    = customer.CustomerId,
                PhotosCount   = customer.PhotosCount,
                Infos         = string.Empty,
                FeedbackCount = customer.FeedbackCount
            };
        }

        public ICustomer ToCommonCustomer()
        {
            return new Common.Customer
            {
                Id          = this.Id,
                CustomerId  = this.CustomerId,
                PhotosCount = this.PhotosCount,
                FeedbackCount = this.FeedbackCount
            };
        }
    }
}

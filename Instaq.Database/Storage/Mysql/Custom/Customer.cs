﻿namespace Instaq.Database
{
    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql;
    using Instaq.Database.Storage.Mysql.Custom;

    public partial class Customer : IIdentifier
    {
        public static Customer FromCommonCustomer(ICustomer customer)
        {
            return new Customer
            {
                Id          = customer.Id,
                CustomerId  = customer.CustomerId,
                PhotosCount = customer.PhotosCount,
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

using AutoTagger.Contract;
using AutoTagger.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Database.Storage.Mysql
{
    using System.Linq;

    public class MysqlCustomerStorage : MysqlBaseStorage, ICustomerStorage
    {
        public int Create(ICustomer commonCustomer)
        {
            var customer = Customer.FromCommonCustomer(commonCustomer);
            this.db.Customer.Add(customer);
            this.db.SaveChanges();
            return customer.Id;
        }

        public void Update(ICustomer commonCustomer)
        {
            var customer = Customer.FromCommonCustomer(commonCustomer);
            this.DetachLocal<Customer>(customer, customer.Id);
            this.db.Customer.Update(customer);
            this.db.SaveChanges();
        }

        public bool Exists(string customerId)
        {
            return this.db.Customer.FirstOrDefault(x => x.CustomerId == customerId) != null;
        }
    }
}

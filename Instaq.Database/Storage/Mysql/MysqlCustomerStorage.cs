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

        public void IncreasePhotosCount(string customerId)
        {
            var query = $"UPDATE `customer` SET photos_count = photos_count+1 WHERE `customer_id`='{customerId}'";
            this.ExecuteCustomQuery(query);
        }

        public void IncreaseFeedbackCount(string customerId)
        {
            var query = $"UPDATE `customer` SET feedback_count = feedback_count+1 WHERE `customer_id`='{customerId}'";
            this.ExecuteCustomQuery(query);
        }

        public void UpdateCustomerId(int id, string customerId)
        {
            var query = $"UPDATE `customer` SET customer_id = '{customerId}' WHERE `id`='{id}'";
            this.ExecuteCustomQuery(query);
        }

        public bool Exists(string customerId)
        {
            return this.db.Customer.FirstOrDefault(x => x.CustomerId == customerId) != null;
        }

        public ICustomer Get(int id)
        {
            return this.db.Customer.FirstOrDefault(x => x.Id == id)?.ToCommonCustomer();
        }
    }
}

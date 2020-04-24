namespace Instaq.Database.Storage.Mysql
{
    using System;

    using Instaq.Contract;
    using Instaq.Contract.Models;
    using System.Linq;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlCustomerStorage : MysqlBaseStorage, ICustomerStorage
    {

        public MysqlCustomerStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public int Create(ICustomer commonCustomer)
        {
            var customer = Customer.FromCommonCustomer(commonCustomer);
            this.Db.Customer.Add(customer);
            this.Db.SaveChanges();
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

        public void UpdateInfos(string customerId, string data)
        {
            var customer = this.Db.Customer.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
            {
                throw new ArgumentException();
            }
            customer.Infos = data;
            this.Db.Customer.Update(customer);
            this.Db.SaveChanges();
        }

        public bool Exists(string customerId)
        {
            return this.Db.Customer.FirstOrDefault(x => x.CustomerId == customerId) != null;
        }

        public ICustomer Get(int id)
        {
            return this.Db.Customer.FirstOrDefault(x => x.Id == id)?.ToCommonCustomer();
        }
    }
}

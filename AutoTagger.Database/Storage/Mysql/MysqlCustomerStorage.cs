using AutoTagger.Contract;
using AutoTagger.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Database.Storage.Mysql
{
    public class MysqlCustomerStorage : MysqlBaseStorage, ICustomerStorage
    {
        public ICustomer Create(ICustomer customer)
        {
            //this.db.Blacklist.Add(customer);
            //this.db.SaveChanges();
            return customer;
        }

        public void Update(ICustomer customer)
        {
            //this.db.Blacklist.Update(customer);
            //this.db.SaveChanges();
        }
    }
}

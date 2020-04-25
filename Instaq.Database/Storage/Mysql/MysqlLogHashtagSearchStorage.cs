namespace Instaq.Database.Storage.Mysql
{
    using Instaq.Contract;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlLogHashtagSearchStorage : MysqlBaseStorage, ILogHashtagSearchStorage
    {
        public MysqlLogHashtagSearchStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public int InsertLog(string type, string data, string customerId)
        {
            var debug = new LogsHashtagSearch
            {
                Type = type,
                Data = data,
                CustomerId = customerId
            };
            this.Db.LogsHashtagSearch.Add(debug);
            this.Db.SaveChanges();
            return debug.Id;
        }
    }
}

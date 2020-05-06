namespace Instaq.Database.Storage.Mysql
{
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlLogSystem : MysqlBaseStorage, ILogSystem
    {
        public MysqlLogSystem(InstaqContext context)
            : base(context)
        {
        }

        public void InsertLog(string logLevel, string data)
        {
            var dto = new LogsSystem
            {
                LogLevel = logLevel,
                Data = data
            };
            this.Db.LogsSystem.Add(dto);
            this.Db.SaveChanges();
        }
    }
}

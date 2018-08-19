namespace AutoTagger.Database.Storage.Mysql
{
    using System.Linq;

    using AutoTagger.Contract.Storage;

    public class MysqlDebugStorage : MysqlBaseStorage, IDebugStorage
    {

        public string GetPhotosCount()
        {
            var query = "SELECT count(*) as count from photos";
            var results = this.ExecuteCustomQuery(query);
            return results?.FirstOrDefault()?.FirstOrDefault();
        }

    }

}

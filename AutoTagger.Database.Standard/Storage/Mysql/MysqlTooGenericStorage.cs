using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Database.Storage.Mysql
{
    using AutoTagger.Contract;

    public class MysqlTooGenericStorage : MysqlBaseStorage, ITooGenericStorage
    {
        public IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0)
        {
            var query = $"SELECT name, posts, id FROM itags ORDER BY id ASC LIMIT {lastId}, {count}";
            return ExecuteHTagsQuery(query);
        }
    }
}

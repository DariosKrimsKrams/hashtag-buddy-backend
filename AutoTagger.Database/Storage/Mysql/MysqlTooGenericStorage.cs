namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public class MysqlTooGenericStorage : MysqlBaseStorage, ITooGenericStorage
    {
        public int CountHumanoidTagsForHumanoidTag(string name)
        {
            var query = "SELECT count(*) FROM ( SELECT rel2.itagid "
                      + "FROM photo_itag_rel as rel2 LEFT JOIN ( SELECT photoId as pId "
                      + "FROM photo_itag_rel as rel WHERE rel.itagId = ( SELECT i.id FROM "
                      + $"itags as i WHERE name = '{name}' LIMIT 1)) as sub ON sub.pId = "
                      + "rel2.photoId WHERE sub.pId IS NOT NULL GROUP by rel2.itagId ) final";
            var (results, time) = this.ExecuteCustomQuery(query);
            var result  = Convert.ToInt32(results.FirstOrDefault()?.FirstOrDefault());
            return result;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0)
        {
            var query = $"SELECT id, name, posts FROM itags ORDER BY id ASC LIMIT {lastId}, {count}";
            return this.ExecuteHTagsQuery(query).Item1;
        }

        public void UpdateRefCount(IHumanoidTag humanoidTag)
        {
            var query = $"UPDATE itags SET `refCount`='{humanoidTag.RefCount}' "
                      + $"WHERE `id`='{humanoidTag.Id}' LIMIT 1";
            this.ExecuteCustomQuery(query);
        }
    }
}

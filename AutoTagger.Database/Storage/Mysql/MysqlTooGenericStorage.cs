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
            var query = "SELECT count(*) FROM ("
                      + "SELECT rel2.itag FROM photo_itag_rel as rel2 LEFT JOIN( "
                      + "SELECT shortcode, itag as pId "
                      + "FROM photo_itag_rel as rel WHERE rel.itag = ( "
                      + "SELECT i.name FROM itags as i "
                      + $"WHERE name = '{name}' LIMIT 1) "
                      + ") as sub  ON sub.shortcode = rel2.shortcode "
                      + "WHERE sub.pId IS NOT NULL GROUP by rel2.itag ) final ";
            var (results, time) = this.ExecuteCustomQuery(query);
            var result  = Convert.ToInt32(results.FirstOrDefault()?.FirstOrDefault());
            return result;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0)
        {
            var query = $"SELECT name, posts FROM itags ORDER BY name ASC LIMIT {lastId}, {count}";
            return this.ExecuteHTagsQuery(query).Item1;
        }

        public void UpdateRefCount(IHumanoidTag humanoidTag)
        {
            var query = $"UPDATE itags SET `refCount`='{humanoidTag.RefCount}' "
                      + $"WHERE `name`='{humanoidTag.Name}' LIMIT 1";
            this.ExecuteCustomQuery(query);
        }
    }
}

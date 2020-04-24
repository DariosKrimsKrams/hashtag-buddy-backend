namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlTooGenericStorage : MysqlBaseStorage, ITooGenericStorage
    {

        public MysqlTooGenericStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public int CountHumanoidTagsForHumanoidTag(string name)
        {
            var query = "SELECT count(*) FROM ("
                      + "SELECT rel2.itag FROM photo_itag_rel as rel2 LEFT JOIN( "
                      + "SELECT shortcode, itag as pId "
                      + $"FROM photo_itag_rel as rel WHERE rel.itag = '{name}' "
                      + ") as sub  ON sub.shortcode = rel2.shortcode "
                      + "WHERE sub.pId IS NOT NULL GROUP by rel2.itag ) final ";
            var (results, _) = this.ExecuteCustomQuery(query);
            var result  = Convert.ToInt32(results.FirstOrDefault()?.FirstOrDefault());
            return result;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int limitSkip = 0)
        {
            var query = $"SELECT name, posts FROM itags ORDER BY name ASC LIMIT {limitSkip}, {count}";
            return this.ExecuteHTagsQuery(query).Item1;
        }

        public IEnumerable<IHumanoidTag> GetHumanoidTagsWithNoRefCount(int count, int limitSkip = 0)
        {
            var query = "SELECT * FROM `itags` WHERE `refCount` = '0'"
                      + $"ORDER BY `posts` desc LIMIT { limitSkip}, {count}";
            return this.ExecuteHTagsQuery(query).Item1;
        }

        public void UpdateRefCount(IHumanoidTag humanoidTag)
        {
            var query = $"UPDATE `itags` SET `refCount`='{humanoidTag.RefCount}' "
                      + $"WHERE `name`='{humanoidTag.Name}' LIMIT 1";
            this.ExecuteCustomQuery(query);
        }
    }
}

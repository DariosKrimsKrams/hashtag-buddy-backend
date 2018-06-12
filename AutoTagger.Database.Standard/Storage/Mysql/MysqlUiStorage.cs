namespace AutoTagger.Database.Standard.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::AutoTagger.Contract;
    using global::AutoTagger.Database.Standard.Mysql;

    using Microsoft.EntityFrameworkCore;

    public class MysqlUIStorage : MysqlBaseStorage, IAutoTaggerStorage
    {
        public (string debug, IEnumerable<IEnumerable<string>> htags) FindHumanoidTags(IEnumerable<IMTag> machineTags)
        {
            var query = BuildQuery(machineTags);
            var htags = this.ExecuteCustomQuery(query);
            return (query, htags);
        }

        private string BuildQuery(IEnumerable<IMTag> machineTags)
        {
            var countInsertTags   = machineTags.Count();
            var limitTopPhotos    = 200;
            var countTagsToReturn = 30;
            var whereConditionLabel = BuildWhereCondition(machineTags, "GCPVision_Label");
            var whereConditionWeb = BuildWhereCondition(machineTags, "GCPVision_Web");

            string query = $"SELECT i.name, i.posts "
                         + $"FROM itags AS i LEFT JOIN photo_itag_rel AS rel ON rel.itagId = i.id LEFT JOIN "
                         + $"( SELECT p.id, ((count(m.name)-2 * matches + {countInsertTags}) / (count(m.name) "
                         + $"+ {countInsertTags} - matches)) *popularity as relationQuality "
                         + $"FROM photos AS p LEFT JOIN mtags AS m ON m.photoId = p.id LEFT JOIN "
                         + $"( SELECT p.id, (p.likes+p.comments)/ p.follower AS popularity, count(m.name) AS matches "
                         + $"FROM photos as p LEFT JOIN mtags AS m ON m.photoId = p.id "
                         + $"WHERE (({whereConditionLabel}) AND m.source='GCPVision_Label') "
                         + $"OR (({whereConditionWeb}) AND m.source='GCPVision_Web') "
                         + $"GROUP by p.id ORDER BY matches DESC LIMIT {limitTopPhotos}) AS sub1 ON p.id = sub1.id WHERE sub1.id IS NOT NULL "
                         + $"GROUP by p.id ORDER BY relationQuality DESC LIMIT {limitTopPhotos} ) AS sub2 ON sub2.id = rel.photoId "
                         + $"WHERE sub2.id IS NOT NULL GROUP by i.name ORDER by count(i.name) DESC, relationQuality DESC LIMIT {countTagsToReturn}";

            return query;
        }

        private static string BuildWhereCondition(IEnumerable<IMTag> machineTags, string source)
        {
            var whereCondition = "";
            foreach (var machineTag in machineTags)
            {
                if (machineTag.Source != source)
                    continue;
                if (string.IsNullOrEmpty(machineTag.Name))
                    continue;
                whereCondition += $"`m`.`name` = '{machineTag.Name.Replace("'", "\\'")}' OR ";
            }

            char[] charsToTrim = { ' ', 'O', 'R' };
            whereCondition     = whereCondition.Trim(charsToTrim);
            return whereCondition;
        }

        public void Log(string source, string data)
        {
            var debug = new Debug { Source = source, Data = data};
            this.db.Debug.Add(debug);
            this.db.SaveChanges();
        }

        public IEnumerable<IEnumerable<string>> GetMtagsWithHighScore()
        {
            var query = "SELECT m.name, MAX(m.score), count(m.name) "
                      + "FROM mtags as m "
                      + "WHERE source = 'GCPVision_Web' "
                      + "AND m.score > 5 "
                      + "GROUP BY m.name "
                      + "ORDER by MAX(m.score) DESC";
            var mTags = this.ExecuteCustomQuery(query);
            return mTags;
        }
    }
}

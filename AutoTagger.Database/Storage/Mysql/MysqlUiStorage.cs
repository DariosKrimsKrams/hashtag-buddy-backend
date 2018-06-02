namespace AutoTagger.Database.Storage.AutoTagger
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::AutoTagger.Contract;
    using global::AutoTagger.Database.Mysql;

    using Microsoft.EntityFrameworkCore;

    public class MysqlUIStorage : MysqlBaseStorage, IAutoTaggerStorage
    {
        public (string debug, IEnumerable<string> htags) FindHumanoidTags(List<IMTag> machineTags)
        {
            var query = BuildQuery(machineTags);
            var htags = this.ExecuteCustomQuery(query);
            return (query, htags);
        }

        private string BuildQuery(IEnumerable<IMTag> machineTags)
        {
            var limitTopPhotos    = 100;
            var countTagsToReturn = 30;
            var whereConditionLabel = BuildWhereCondition(machineTags, "GCPVision_Label");
            var whereConditionWeb = BuildWhereCondition(machineTags, "GCPVision_Web");

            var query = $"SELECT i.name "
                        + $"FROM itags as i LEFT JOIN photo_itag_rel as rel ON rel.itagId = i.id "
                        + $"LEFT JOIN (SELECT p.id, count(m.name) as matches FROM photos as p "
                        + $"LEFT JOIN mtags as m ON m.photoId = p.id "
                        + $"WHERE (({whereConditionLabel}) AND m.source='GCPVision_Label')"
                        + $"OR (({whereConditionWeb}) AND m.source='GCPVision_Web')"
                        + $" GROUP BY p.id ORDER BY matches DESC LIMIT {limitTopPhotos} "
                        + $") as sub2 ON sub2.id = rel.photoId WHERE sub2.id IS NOT NULL "
                        + $"GROUP BY i.name ORDER by sum(matches) DESC LIMIT {countTagsToReturn}";
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

        public IEnumerable<string> GetMtagsWithHighScore()
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

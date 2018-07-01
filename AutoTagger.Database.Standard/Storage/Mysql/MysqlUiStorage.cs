namespace AutoTagger.Database.Standard.Storage.Mysql
{
    using System.Collections.Generic;
    using System.Linq;
    using global::AutoTagger.Contract;
    using global::AutoTagger.Database.Standard.Mysql;

    public class MysqlUIStorage : MysqlBaseStorage, IAutoTaggerStorage
    {

        public (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags(IEnumerable<IMachineTag> machineTags)
        {
            var query = BuildQueryWithUserRelevance(machineTags);
            var htags = this.ExecuteHTagsQuery(query);
            return (query, htags);
        }

        public (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> machineTags)
        {
            var queryTrending = this.BuildQueryTrending(machineTags);
            var htagsTrending = this.ExecuteHTagsQuery(queryTrending);
            return (queryTrending, htagsTrending);
        }

        private static (string, string) BuildWhereConditions(IEnumerable<IMachineTag> machineTags)
        {
            var whereConditionLabel = BuildWhereCondition(machineTags, "GCPVision_Label");
            var whereConditionWeb   = BuildWhereCondition(machineTags, "GCPVision_Web");
            return (whereConditionLabel, whereConditionWeb);
        }

        private string BuildQueryWithUserRelevance(IEnumerable<IMachineTag> machineTags)
        {
            const int limitTopPhotos    = 200;
            const int countTagsToReturn = 30;
            var countInsertTags   = machineTags.Count();
            var (whereConditionLabel, whereConditionWeb) = BuildWhereConditions(machineTags);

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

        private string BuildQueryTrending(IEnumerable<IMachineTag> machineTags)
        {
            const int limitTopPhotos    = 50;
            const int countTagsToReturn = 30;
            var (whereConditionLabel, whereConditionWeb) = BuildWhereConditions(machineTags);

            var query = $"SELECT i.name, i.posts "
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

        private static string BuildWhereCondition(IEnumerable<IMachineTag> machineTags, string source)
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

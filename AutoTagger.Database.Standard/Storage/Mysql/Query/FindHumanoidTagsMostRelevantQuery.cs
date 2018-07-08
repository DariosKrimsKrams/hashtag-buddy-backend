namespace AutoTagger.Database.Storage.Mysql.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;

    public class FindHumanoidTagsMostRelevantQuery : FindHumanoidTagsQueryBase
    {
        public override string GetQuery(IEnumerable<IMachineTag> machineTags)
        {
            const int limitTopPhotos    = 200;
            const int countTagsToReturn = 30;
            var       countInsertTags   = machineTags.Count();
            var (whereConditionLabel, whereConditionWeb) = BuildWhereConditions(machineTags);
            var usageITagsLimit = 10 * 1000;

            string query = $"SELECT i.name, i.posts, i.id, i.amountOfUsageWithOtherITags "
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
                         + $"WHERE sub2.id IS NOT NULL AND i.amountOfUsageWithOtherITags < {usageITagsLimit} GROUP by i.name "
                         + $"ORDER by count(i.name) DESC, relationQuality DESC LIMIT {countTagsToReturn}";

            return query;
        }
    }
}

namespace AutoTagger.Database.Storage.Mysql.Query
{
    using System.Collections.Generic;
    using AutoTagger.Contract;

    public class FindHumanoidTagsTrendingQuery : FindHumanoidTagsQueryBase
    {
        public override string GetQuery(IEnumerable<IMachineTag> machineTags)
        {
            const int limitTopPhotos    = 50;
            const int countTagsToReturn = 30;
            var (whereConditionLabel, whereConditionWeb) = BuildWhereConditions(machineTags);
            var usageITagsLimit = 5 * 1000;

            var query = $"SELECT i.name, i.posts, i.refCount "
                      + $"FROM itags as i LEFT JOIN photo_itag_rel as rel ON rel.itagId = i.id "
                      + $"LEFT JOIN (SELECT p.id, count(m.name) as matches FROM photos as p "
                      + $"LEFT JOIN mtags as m ON m.photoId = p.id "
                      + $"WHERE (({whereConditionLabel}) AND m.source='GCPVision_Label')"
                      + $"OR (({whereConditionWeb}) AND m.source='GCPVision_Web')"
                      + $" GROUP BY p.id ORDER BY matches DESC LIMIT {limitTopPhotos} "
                      + $") as sub2 ON sub2.id = rel.photoId WHERE sub2.id IS NOT NULL "
                      + $"AND i.refCount < {usageITagsLimit} "
                      + $"GROUP BY i.name ORDER by sum(matches) DESC LIMIT {countTagsToReturn}";

            return query;
        }
    }
}

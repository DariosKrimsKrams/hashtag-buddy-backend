namespace AutoTagger.Database.Storage.Mysql.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public class FindHumanoidTagsMostRelevantQuery : FindHumanoidTagsQueryBase
    {
        public override string GetQuery(IMachineTag[] machineTags)
        {
            const int LimitTopPhotos    = 200;
            const int CountTagsToReturn = 30;
            var       countInsertTags   = machineTags.Length;
            var (whereConditionLabel, whereConditionWeb) = BuildWhereConditions(machineTags);

            var query = $"SELECT i.name, i.posts, i.refCount, relationQuality "
                      + $"FROM itags as i JOIN photo_itag_rel as rel ON rel.itag = i.name "
                      + $"JOIN ( SELECT p.shortcode, ((count(m.name) - 2 * matches + {countInsertTags}) / "
                      + $"(count(m.name) + {countInsertTags} - matches)) * popularity as relationQuality "
                      + $"FROM photos as p JOIN mtags as m ON m.shortcode = p.shortcode "
                      + $"JOIN ( SELECT p.shortcode, (p.likes + p.comments) / p.follower as popularity, "
                      + $"count(m.name) as matches FROM photos as p JOIN mtags as m ON "
                      + $"m.shortcode = p.shortcode WHERE((({whereConditionLabel}) AND m.source = 'GCPVision_Label') "
                      + $"OR(({whereConditionWeb}) AND m.source = 'GCPVision_Web') ) AND m.onBlacklist = '0' "
                      + $"GROUP BY p.shortcode ORDER by matches DESC LIMIT {LimitTopPhotos} ) as sub1 ON "
                      + $"p.shortcode = sub1.shortcode WHERE m.onBlacklist = '0' GROUP BY p.shortcode "
                      + $"ORDER BY relationQuality DESC LIMIT {LimitTopPhotos} ) as sub2 ON sub2.shortcode = rel.shortcode "
                      + $"WHERE i.refCount < {RefCountLimit} AND i.onBlacklist = 0 GROUP BY i.name "
                      + $"ORDER BY count(i.name) DESC, relationQuality DESC LIMIT {CountTagsToReturn}";

            return query;
        }
    }
}

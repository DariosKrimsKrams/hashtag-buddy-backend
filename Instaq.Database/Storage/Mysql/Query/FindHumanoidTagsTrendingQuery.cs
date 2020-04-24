namespace Instaq.Database.Storage.Mysql.Query
{
    using Instaq.Contract.Models;

    public class FindHumanoidTagsTrendingQuery : FindHumanoidTagsQueryBase
    {
        public override string GetQuery(IMachineTag[] machineTags)
        {
            const int LimitTopPhotos    = 50;
            const int CountTagsToReturn = 30;
            var whereConditionLabel = BuildWhereConditions(machineTags);

            var query = $"SELECT i.name, i.posts FROM itags as i JOIN photo_itag_rel as rel "
                      + $"ON rel.itag = i.name JOIN ( SELECT p.shortcode, count(m.name) as matches "
                      + $"FROM photos as p JOIN mtags as m ON m.shortcode = p.shortcode WHERE "
                      + $"(({whereConditionLabel}) AND m.source = 'GCPVision_Label') "
                      + $"AND m.onBlacklist = '0' "
                      + $"GROUP BY p.shortcode ORDER BY matches DESC LIMIT {LimitTopPhotos} ) as sub2 ON "
                      + $"sub2.shortcode = rel.shortcode WHERE i.refCount < {RefCountLimit} AND "
                      + $"i.onBlacklist = '0' GROUP BY i.name ORDER BY sum(matches) DESC LIMIT {CountTagsToReturn}";

            return query;
        }
    }
}

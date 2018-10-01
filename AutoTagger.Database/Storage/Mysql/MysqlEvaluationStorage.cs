namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Database.Storage.Mysql.Query;

    public class MysqlEvaluationStorage : MysqlBaseStorage, IEvaluationStorage
    {

        public (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IMachineTag[] machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsMostRelevantQuery>(machineTags);
        }

        public (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IMachineTag[] machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsTrendingQuery>(machineTags);
        }

        private (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags<T>(IMachineTag[] machineTags) where T : IFindHumanoidTagsQuery
        {
            var instance     = Activator.CreateInstance<T>();
            var query        = instance.GetQuery(machineTags);
            var (humanoidTags, _) = this.ExecuteHTagsQuery(query);
            return (query, humanoidTags);
        }

        public IEnumerable<IEnumerable<string>> GetMtagsWithHighScore()
        {
            var query = "SELECT m.name, MAX(m.score), count(m.name) "
                      + "FROM mtags as m "
                      + "WHERE source = 'GCPVision_Web' "
                      + "AND m.score > 5 "
                      + "GROUP BY m.name "
                      + "ORDER by MAX(m.score) DESC";
            var (mTags, time)  = this.ExecuteCustomQuery(query);
            return mTags;
        }
    }
}

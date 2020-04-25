namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using Instaq.Common.Dto;
    using Instaq.Contract;
    using Instaq.Contract.Dto;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql.Generated;
    using Instaq.Database.Storage.Mysql.Query;

    public class MysqlEvaluationStorage : MysqlBaseStorage, IEvaluationStorage
    {

        public MysqlEvaluationStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public IEvaluationDto FindMostRelevantHumanoidTags(IMachineTag[] machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsMostRelevantQuery>(machineTags);
        }

        public IEvaluationDto FindTrendingHumanoidTags(IMachineTag[] machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsTrendingQuery>(machineTags);
        }

        public IEvaluationDto FindHumanoidTags<T>(IMachineTag[] machineTags) where T : IFindHumanoidTagsQuery
        {
            var instance     = Activator.CreateInstance<T>();
            var query        = instance.GetQuery(machineTags);
            var (humanoidTags, time) = this.ExecuteHTagsQuery(query);
            return new EvaluationDto
            {
                Query = query,
                HumanoidTags = humanoidTags,
                TimeNeeded = time.TotalSeconds
            };
        }

        public IEnumerable<IEnumerable<string>> GetMtagsWithHighScore()
        {
            var query = "SELECT m.name, MAX(m.score), count(m.name) "
                      + "FROM mtags as m "
                      + "WHERE source = 'GCPVision_Web' "
                      + "AND m.score > 5 "
                      + "GROUP BY m.name "
                      + "ORDER by MAX(m.score) DESC";
            var (mTags, _)  = this.ExecuteCustomQuery(query);
            return mTags;
        }
    }
}

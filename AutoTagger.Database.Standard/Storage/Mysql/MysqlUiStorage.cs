namespace AutoTagger.Database.Standard.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Database.Standard.Mysql;
    using AutoTagger.Database.Standard.Storage.Mysql.Query;

    public class MysqlUiStorage : MysqlBaseStorage, IUiStorage
    {

        public (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IEnumerable<IMachineTag> machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsMostRelevantQuery>(machineTags);
        }

        public (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IEnumerable<IMachineTag> machineTags)
        {
            return this.FindHumanoidTags<FindHumanoidTagsTrendingQuery>(machineTags);
        }

        private (string debug, IEnumerable<IHumanoidTag> htags) FindHumanoidTags<T>(IEnumerable<IMachineTag> machineTags) where T : IFindHumanoidTagsQuery
        {
            var instance     = Activator.CreateInstance<T>();
            var query        = instance.GetQuery(machineTags);
            var humanoidTags = this.ExecuteHTagsQuery(query);
            return (query, humanoidTags);
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

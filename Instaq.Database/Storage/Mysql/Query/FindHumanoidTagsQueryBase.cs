
namespace AutoTagger.Database.Storage.Mysql.Query
{
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public abstract class FindHumanoidTagsQueryBase : IFindHumanoidTagsQuery
    {
        public abstract string GetQuery(IMachineTag[] machineTags);
        protected const int RefCountLimit = 30000;

        protected static (string, string) BuildWhereConditions(IMachineTag[]  machineTags)
        {
            var whereConditionLabel = BuildWhereCondition(machineTags, "GCPVision_Label");
            var whereConditionWeb   = BuildWhereCondition(machineTags, "GCPVision_Web");
            return (whereConditionLabel, whereConditionWeb);
        }

        private static string BuildWhereCondition(IMachineTag[] machineTags, string source)
        {
            var where = "";
            for (var i = 0; i < machineTags.Length; i++)
            {
                var machineTag = machineTags[i];
                if (machineTag.Source != source || string.IsNullOrEmpty(machineTag.Name))
                {
                    continue;
                }
                where += $"`m`.`name` = '{machineTag.Name.Replace("'", @"\'")}' OR ";
            }

            char[] charsToTrim = { ' ', 'O', 'R' };
            return where.TrimEnd(charsToTrim);
        }
    }
}

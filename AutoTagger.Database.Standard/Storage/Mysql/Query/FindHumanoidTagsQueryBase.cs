
namespace AutoTagger.Database.Standard.Storage.Mysql.Query
{
    using System.Collections.Generic;
    using AutoTagger.Contract;

    public abstract class FindHumanoidTagsQueryBase : IFindHumanoidTagsQuery
    {
        public abstract string GetQuery(IEnumerable<IMachineTag> machineTags);

        protected static (string, string) BuildWhereConditions(IEnumerable<IMachineTag> machineTags)
        {
            var whereConditionLabel = BuildWhereCondition(machineTags, "GCPVision_Label");
            var whereConditionWeb   = BuildWhereCondition(machineTags, "GCPVision_Web");
            return (whereConditionLabel, whereConditionWeb);
        }

        private static string BuildWhereCondition(IEnumerable<IMachineTag> machineTags, string source)
        {
            var where = "";
            foreach (var machineTag in machineTags)
            {
                if (machineTag.Source != source)
                    continue;
                if (string.IsNullOrEmpty(machineTag.Name))
                    continue;
                where += $"`m`.`name` = '{machineTag.Name.Replace("'", "\\'")}' OR ";
            }
            char[] charsToTrim = { ' ', 'O', 'R' };
            return where.Trim(charsToTrim);
        }
    }
}

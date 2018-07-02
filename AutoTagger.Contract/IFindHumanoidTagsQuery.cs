using System.Collections.Generic;

namespace AutoTagger.Contract
{
    public interface IFindHumanoidTagsQuery
    {
        string GetQuery(IEnumerable<IMachineTag> machineTags);
    }
}

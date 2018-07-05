namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IFindHumanoidTagsQuery
    {
        string GetQuery(IEnumerable<IMachineTag> machineTags);
    }
}

namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface IFindHumanoidTagsQuery
    {
        string GetQuery(IMachineTag[] machineTags);
    }
}

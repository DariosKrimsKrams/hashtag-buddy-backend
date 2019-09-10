namespace Instaq.Contract
{
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface IFindHumanoidTagsQuery
    {
        string GetQuery(IMachineTag[] machineTags);
    }
}

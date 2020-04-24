namespace Instaq.Contract
{
    using Instaq.Contract.Models;

    public interface IFindHumanoidTagsQuery
    {
        string GetQuery(IMachineTag[] machineTags);
    }
}

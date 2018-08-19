namespace AutoTagger.Contract.Storage
{
    public interface IDebugStorage
    {
        string GetPhotosCount();

        string GetHumanoidTagsCount();

        string GetHumanoidTagRelationCount();

        string GetMachineTagsCount();
    }
}

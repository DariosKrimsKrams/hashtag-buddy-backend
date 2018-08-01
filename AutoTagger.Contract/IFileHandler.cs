using System.Collections.Generic;

namespace AutoTagger.Contract
{
    public interface IFileHandler
    {
        IList<string> GetAllUnusedImages();

        bool FileExists(string filename);

        void FlagAsUsed(string filename);

        void FlagAsDefect(string filename);

        void Delete(string filename);

        string GetFullPath(string filename);

        int GetFileSize(string filename);

        void Save(FolderType user, byte[] bytes, string filename);
    }
}

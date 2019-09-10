using System.Collections.Generic;

namespace Instaq.Contract
{
    public interface IFileHandler
    {
        IList<string> GetAllUnusedImages();

        bool FileExists(FileType fileType, string name);

        void FlagAsUsed(string filename);

        void FlagAsDefect(string filename);

        void Delete(string filename);

        string GetFullPath(string filename);

        int GetFileSize(FileType fileType, string filename);

        void Save(FileType user, byte[] bytes, string filename);

        byte[] GetFile(FileType fileType, string fileNameAndExt);
    }
}

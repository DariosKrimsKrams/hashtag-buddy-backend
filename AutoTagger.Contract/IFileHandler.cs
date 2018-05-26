using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract.Standard
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
    }
}

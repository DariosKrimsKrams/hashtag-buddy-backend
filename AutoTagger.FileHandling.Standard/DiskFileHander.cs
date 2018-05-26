namespace AutoTagger.FileHandling.Standard
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AutoTagger.Contract.Standard;

    public class DiskFileHander : IFileHandler
    {
        private readonly string PathUnused = @"C:\Instagger\Unused\";
        private readonly string PathUsed = @"C:\Instagger\Used\";
        private readonly string PathDefect = @"C:\Instagger\Defect\";
        private readonly string Ext = ".jpg";

        public IList<string> GetAllUnusedImages()
        {
            var files = Directory.GetFiles(this.PathUnused, "*" + this.Ext);
            return files.Select(x => x.Replace(this.PathUnused, "").Replace(this.Ext, "")).ToList();
        }

        public bool FileExists(string name)
        {
            var path = this.PathUnused + name + Ext;
            return File.Exists(path);
        }

        public void FlagAsUsed(string name)
        {
            var fromPath = this.PathUnused + name + Ext;
            var toPath = this.PathUsed + name + Ext;
            File.Move(fromPath, toPath);
        }

        public void FlagAsDefect(string name)
        {
            var fromPath = this.PathUnused + name + Ext;
            var toPath   = this.PathDefect + name + Ext;
            File.Move(fromPath, toPath);
        }

        public void Delete(string name)
        {
            var path = this.PathUnused + name + Ext;
            File.Delete(path);
        }

        public string GetFullPath(string name)
        {
            return this.PathUnused + name + Ext;
        }

        public int GetFileSize(string filename)
        {
            var path = this.PathUnused + filename + Ext;
            return File.ReadAllBytes(path).Length;
        }
    }
}

using System;

namespace Instaq.BlacklistImport
{
    using System.Text.RegularExpressions;

    public class Importer
    {
        public Importer(string filePath)
        {
            var fh = new FileHandler();
            var entries = fh.ReadCsv(filePath);

            
        }
    }
}

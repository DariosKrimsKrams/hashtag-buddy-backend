namespace Instaq.BlacklistImport
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    class CsvImporter
    {
        public IEnumerable<string> ReadFile(string filePath)
        {
            //var fh = new FileHandler();
            //var entries = fh.ReadCsv(filePath);
            //return entries;
            return new List<string>();
        }
    }
}

namespace Instaq.BlacklistImport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    class CsvImporter
    {
        public IEnumerable<string> ReadFile(string filePath)
        {
            var entries = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line   = reader.ReadLine();
                    var values = line.Split(',');
                    foreach (var value in values)
                    {
                        if (string.IsNullOrEmpty(value))
                            continue;
                        entries.Add(value);
                    }
                }
            }
            return entries;
        }
    }
}

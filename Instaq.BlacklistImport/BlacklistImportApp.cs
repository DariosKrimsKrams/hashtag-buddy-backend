namespace Instaq.BlacklistImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Instaq.Common;
    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;

    public class BlacklistImportApp
    {
        private readonly IBlacklistStorage db;
        private readonly CsvImporter importer;
        private readonly TextBuilder textBuilder;

        public BlacklistImportApp(IBlacklistStorage db)
        {
            this.db          = db;
            this.importer    = new CsvImporter();
            this.textBuilder = new TextBuilder();
        }

        public void ReadCsv(string filename, string reason, string table)
        {
            this.db.Delete(reason, table);
            var rawEntries = this.importer.ReadFile(filename);

            var entries = this.textBuilder.GetCleanList(rawEntries);
            var items = new List<IBlacklistEntry>();
            foreach (var entry in entries)
            {
                var item = new BlacklistEntry {
                    Name = entry,
                    Reason = reason,
                    Table = table
                };
                items.Add(item);
            }
            this.db.Insert(items);
        }

        public void SetBlacklistFlags()
        {
            this.DoTag("itags");
            this.DoTag("mtags");
        }

        private void DoTag(string tableName)
        {
            var overallCount = 0;
            var entriesCount = 0;
            do
            {
                Console.WriteLine("Get Data");
                var result = this.db.GetTags(tableName, 1);
                var enumerable = result.tags as ITag[] ?? result.tags.ToArray();
                entriesCount = enumerable.Count();
                Console.WriteLine($"Got Data: {entriesCount} entries (time: {result.time})");
                overallCount += entriesCount;
                this.db.UpdateTags(enumerable, tableName);
                Console.WriteLine($"Updated tags: {enumerable.Count()} | Table: {tableName} | Count: {overallCount}" +
                                  $" | Some Tags: {enumerable[0].Name}");

            } while (entriesCount != 0);
        }
    }
}

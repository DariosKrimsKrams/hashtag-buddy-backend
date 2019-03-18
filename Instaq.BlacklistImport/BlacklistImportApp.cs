namespace Instaq.BlacklistImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

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

        public void SetItagOnBlacklistFlags()
        {
            var entries = this.db.GetAllBlacklistEntries();
            var iTags = new List<string>();
            var mTags = new List<string>();
            var countBlacklistEntries = 0;
            var pendingUpdates = 0;
            var rowsNothingHappened = 0;
            var overallCount = entries.Count();
            foreach (var entry in entries)
            {
                countBlacklistEntries++;
                var hasUpdate = false;

                IEnumerable<IEntity> tags;
                if (entry.Table == "mtags")
                {
                    mTags.Add(entry.Name);
                }
                else
                {
                    iTags.Add(entry.Name);
                }
                pendingUpdates++;
                hasUpdate = true;

                if (!hasUpdate)
                {
                    rowsNothingHappened++;
                }
                if (pendingUpdates >= 100)
                {
                    pendingUpdates = 0;
                    this.Update(iTags, mTags, countBlacklistEntries, overallCount);
                    iTags.Clear();
                    mTags.Clear();
                }
                if (rowsNothingHappened >= 50)
                {
                    rowsNothingHappened = 0;
                    Console.WriteLine("No Updates (BlacklistEntries: " + countBlacklistEntries + "/" + overallCount + ")");
                }
            }

            this.Update(iTags, mTags, countBlacklistEntries, overallCount);
        }

        private void Update(IEnumerable<string> iTags, IEnumerable<string> mTags, int countBlacklistEntries, int overallCount)
        {
            this.db.UpdateTags(iTags, "itags");
            this.db.UpdateTags(mTags, "mtags");
            Console.WriteLine("Updated ITags: " + iTags.Count() + " updated mTags: " + mTags.Count() + " (BlacklistEntries: " + countBlacklistEntries + "/" + overallCount + ")");
        }
    }
}

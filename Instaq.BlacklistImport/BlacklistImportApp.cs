namespace Instaq.BlacklistImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;

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

        public void ReadCsv(string filename, string reason="days", string table = "itags")
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
            var blacklistEntries = this.db.GetAllBlacklistEntries();
            var updatedEntries = new List<IHumanoidTag>();
            var countBlacklistEntries = 0;
            var pendingUpdates = 0;
            foreach (var blacklistEntry in blacklistEntries)
            {
                if (blacklistEntry.Table == "mtags")
                {
                    // ToDo
                    //var mTags = this.db.GetMachineTagsThatContain(blacklistEntry.Name);
                    continue;
                }

                countBlacklistEntries++;
                var hTags = this.db.GetHumanoidTagsThatContain(blacklistEntry.Name);
                foreach (var hTag in hTags)
                {
                    hTag.OnBlacklist = true;
                    updatedEntries.Add(hTag);
                    pendingUpdates++;
                }
                if (pendingUpdates >= 100)
                {
                    pendingUpdates = 0;
                    this.Update(updatedEntries, countBlacklistEntries, blacklistEntries.Count());
                    updatedEntries.Clear();
                }
            }

            this.Update(updatedEntries, countBlacklistEntries, blacklistEntries.Count());
        }

        private void Update(IEnumerable<IHumanoidTag> updatedEntries, int countBlacklistEntries, int overallCount)
        {
            this.db.UpdateHumanoidTags(updatedEntries);
            Console.WriteLine("Updated Itags: " + updatedEntries.Count() + " (BlacklistEntries: " + countBlacklistEntries + "/" + overallCount + ")");
        }
    }
}

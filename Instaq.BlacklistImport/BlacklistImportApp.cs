namespace Instaq.BlacklistImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public void ReadCsv(string filename)
        {
            var rawEntries = this.importer.ReadFile(filename);
            var cleanEntries = this.textBuilder.GetCleanList(rawEntries);
            this.db.Insert(cleanEntries);
        }

        public void SetItagOnBlacklistFlags()
        {
            var blacklistEntries = this.db.GetAllBlacklistEntries();
            var updatedEntries = new List<IHumanoidTag>();
            var countBlacklistEntries = 0;
            var pendingUpdates = 0;
            foreach (var blacklistEntry in blacklistEntries)
            {
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

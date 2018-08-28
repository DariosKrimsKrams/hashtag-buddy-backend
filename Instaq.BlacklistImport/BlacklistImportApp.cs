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

        public void ReadCsv(string filename, string reason="days", string table = "iTags")
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
            var iTags = new List<IEntity>();
            var mTags = new List<IEntity>();
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
                    tags = this.db.GetMachineTagsThatContain(entry.Name);
                    foreach (var tag in tags)
                    {
                        mTags.Add(tag);
                        pendingUpdates++;
                        hasUpdate = true;
                    }
                }
                else
                {
                    tags = this.db.GetHumanoidTagsThatContain(entry.Name);
                    foreach (var tag in tags)
                    {
                        iTags.Add(tag);
                        pendingUpdates++;
                        hasUpdate = true;
                    }
                }

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

        private void Update(IEnumerable<IEntity> iTags, IEnumerable<IEntity> mTags, int countBlacklistEntries, int overallCount)
        {
            this.db.UpdateTags(iTags, "itags");
            this.db.UpdateTags(mTags, "mtags");
            Console.WriteLine("Updated ITags: " + iTags.Count() + " updated mTags: " + mTags.Count() + " (BlacklistEntries: " + countBlacklistEntries + "/" + overallCount + ")");
        }
    }
}

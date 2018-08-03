namespace Instaq.BlacklistImport
{
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
    }
}

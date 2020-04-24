namespace Instaq.Common
{
    using Instaq.Contract.Models;

    public class MachineTag : IMachineTag
    {
        public MachineTag()
        {
            this.Name = "";
            this.Source = "";
        }

        public MachineTag(string name, float score, string source)
        {
            this.Name   = name;
            this.Score  = score;
            this.Source = source;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public float Score { get; set; }

        public string Source { get; set; }

        public bool OnBlacklist { get; set; }

    }
}

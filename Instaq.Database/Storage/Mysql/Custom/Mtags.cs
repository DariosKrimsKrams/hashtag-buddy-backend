namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Common;
    using Instaq.Contract.Models;

    public partial class Mtags
    {
        public IMachineTag ToMachineTag()
        {
            return new MachineTag
            {
                Id = this.Id,
                Name = this.Name,
                Score = this.Score,
                Source = this.Source,
                OnBlacklist = this.OnBlacklist
            };
        }
    }
}

using System;

namespace Instaq.Database
{
    using Instaq.Common;
    using Instaq.Contract;
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
                OnBlacklist = Convert.ToBoolean(this.OnBlacklist)
            };
        }
    }
}

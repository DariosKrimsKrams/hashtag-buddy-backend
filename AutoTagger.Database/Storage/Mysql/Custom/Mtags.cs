using System;

namespace AutoTagger.Database
{
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

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

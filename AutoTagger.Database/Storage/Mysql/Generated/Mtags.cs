using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    using AutoTagger.Common;
    using AutoTagger.Contract;

    public partial class Mtags
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }
        public string Name { get; set; }
        public float Score { get; set; }
        public string Source { get; set; }
        public sbyte OnBlacklist { get; set; }

        public Photos Photo { get; set; }

        public IMachineTag ToHumanoidTag()
        {
            return new MachineTag
            {
                Id       = this.Id,
                Name     = this.Name,
                Score = this.Score,
                Source = this.Source,
                OnBlacklist = this.OnBlacklist
            };
        }
    }
}

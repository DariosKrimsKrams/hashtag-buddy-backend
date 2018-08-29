using System;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    using AutoTagger.Contract.Models;

    public partial class Debug
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public sbyte Deleted { get; set; }

        public ILog ToLog()
        {
            return new Log {
                Id = this.Id,
                Data = this.Data,
                Created = this.Created,
                Deleted = Convert.ToBoolean(this.Deleted)
            };
        }
    }
}

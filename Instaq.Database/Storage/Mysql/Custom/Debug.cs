namespace AutoTagger.Database
{
    using System;
    using AutoTagger.Contract.Models;

    public partial class Debug
    {
        public ILog ToLog()
        {
            return new Log
            {
                Id      = this.Id,
                Data    = this.Data,
                Created = this.Created,
                Deleted = Convert.ToBoolean(this.Deleted)
            };
        }
    }
}

﻿namespace Instaq.Database.Storage.Mysql.Generated
{
    using System;
    using Instaq.Contract.Models;

    public partial class LogsUpload
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

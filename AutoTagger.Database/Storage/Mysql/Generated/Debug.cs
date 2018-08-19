﻿using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    using AutoTagger.Contract.Models;

    public partial class Debug
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }

        public ILog toLog()
        {
            return new Log { Id = this.Id, Data = this.Data, Created = this.Created };
        }
}
}

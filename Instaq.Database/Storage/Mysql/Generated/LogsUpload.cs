﻿using System;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class LogsUpload
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string CustomerId { get; set; }
        public DateTime Created { get; set; }
        public bool Deleted { get; set; }
    }
}

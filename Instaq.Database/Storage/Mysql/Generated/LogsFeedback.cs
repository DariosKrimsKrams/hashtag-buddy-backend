using System;
using System.Collections.Generic;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class LogsFeedback
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
        public int DebugId { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public bool Deleted { get; set; }
    }
}

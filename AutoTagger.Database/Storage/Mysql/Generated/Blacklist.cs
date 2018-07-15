using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    public partial class Blacklist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
    }
}

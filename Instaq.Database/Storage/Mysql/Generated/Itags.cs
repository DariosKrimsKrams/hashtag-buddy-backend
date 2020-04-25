using System;
using System.Collections.Generic;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class Itags
    {
        public string Name { get; set; }
        public int Posts { get; set; }
        public DateTime Updated { get; set; }
        public int RefCount { get; set; }
        public bool OnBlacklist { get; set; }
    }
}

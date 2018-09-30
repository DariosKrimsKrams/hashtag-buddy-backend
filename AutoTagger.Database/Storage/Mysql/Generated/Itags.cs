using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Itags
    {
        public string Name { get; set; }
        public int Posts { get; set; }
        public DateTime Updated { get; set; }
        public int RefCount { get; set; }
        public sbyte OnBlacklist { get; set; }
    }
}
